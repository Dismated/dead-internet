using DeadInternet.Server.Dtos.Account;
using DeadInternet.Server.Models;

namespace DeadInternet.Server.Interfaces
{
    public interface IAccountService
    {
        Task<AppUser> GetUserByUsernameAsync(string username);
        string CreateToken(AppUser user);
        Task<NewAppUserDto> GetVerifiedUserAsync(LoginDto loginDto);
        string GeneratePassword();
        string GeneratePassword(int length);
        RegisterDto CreateGuestAccount();
        Task CreateAccountAsync(RegisterDto registerDto);
    }
}
