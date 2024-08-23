using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
