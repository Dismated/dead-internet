using LLMForum.Server.Dtos.Account;
using LLMForum.Server.Exceptions;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace LLMForum.Server.Services
{
    public class AccountService(
        IAccountRepository userRepo,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService
    ) : IAccountService
    {
        private readonly IAccountRepository _userRepo = userRepo;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<NewAppUserDto> GetUserByUsernameAsync(LoginDto loginDto)
        {
            var user =
                await _userRepo.GetUserByUsernameAsync(loginDto.Username)
                ?? throw new CustomUnauthorizedAccessException(
                    "Username not found and/or password incorrect!"
                );

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
            return new NewAppUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
            };
        }
    }
}
