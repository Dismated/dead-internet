using LLMForum.Server.Dtos.Account;

namespace LLMForum.Server.Interfaces
{
    public interface IAccountService
    {
        Task<NewAppUserDto> GetUserByUsernameAsync(LoginDto loginDto);
    }
}
