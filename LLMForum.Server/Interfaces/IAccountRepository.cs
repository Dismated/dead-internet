using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface IAccountRepository
    {
        Task<AppUser> GetUserByUsernameAsync(string? username);
    }
}
