using LLMForum.Server.Dtos.Account;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface IAccountService
    {
        Task<AppUser> GetUserByUsernameAsync(string username);
        string CreateToken(AppUser user);
        Task<NewAppUserDto> GetVerifiedUserAsync(LoginDto loginDto);
        string GeneratePassword(int length);
        RegisterDto CreateGuestAccountAsync();
        Task CreateAccountAsync(RegisterDto registerDto);
    }
}
