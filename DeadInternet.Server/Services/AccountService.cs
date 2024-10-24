using DeadInternet.Server.Dtos.Account;
using DeadInternet.Server.Exceptions.Base;
using DeadInternet.Server.Exceptions.User;
using DeadInternet.Server.Interfaces;
using DeadInternet.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace DeadInternet.Server.Services
{
    public class AccountService(
        IAccountRepository userRepo,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        UserManager<AppUser> userManager
    ) : IAccountService
    {
        private readonly IAccountRepository _userRepo = userRepo;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _userRepo.GetUserByUsernameAsync(username)
                ?? throw new CustomUnauthorizedAccessException(
                    "Username not found and/or password incorrect!"
                );
        }

        public string CreateToken(AppUser user)
        {
            return _tokenService.CreateToken(user);
        }

        public async Task<NewAppUserDto> GetVerifiedUserAsync(LoginDto loginDto)
        {
            var user = await GetUserByUsernameAsync(loginDto.Username);
            var signInResult = await _signInManager.CheckPasswordSignInAsync(
                user,
                loginDto.Password,
                false
            );

            if (!signInResult.Succeeded)
                throw new CustomUnauthorizedAccessException(
                    "Username not found and/or password incorrect!"
                );

            if (user.Email == null)
            {
                throw new NotFoundException("Email in AppUser");
            }

            var token = CreateToken(user);

            return new NewAppUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = token,
            };
        }

        public string GeneratePassword()
        {
            return GeneratePassword(12);
        }

        public string GeneratePassword(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string symbols = "!@#$%^&*()";

            var random = new Random();
            var result = new char[length];

            result[0] = char.ToUpper(chars[random.Next(chars.Length)]);
            result[1] = numbers[random.Next(numbers.Length)];
            result[2] = symbols[random.Next(symbols.Length)];

            for (int i = 3; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result.OrderBy(x => random.Next()).ToArray());
        }

        public RegisterDto CreateGuestAccount()
        {
            string guestUsername = $"Guest_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
            string guestPassword = GeneratePassword();

            return new RegisterDto
            {
                Username = guestUsername,
                Email = $"{guestUsername}@guest.com",
                Password = guestPassword,
            };
        }

        public async Task CreateAccountAsync(RegisterDto registerDto)
        {
            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };

            var createResult = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (!createResult.Succeeded)
            {
                throw new UserCreationException(
                    string.Join(", ", createResult.Errors.Select(e => e.Description))
                );
            }
        }
    }
}
