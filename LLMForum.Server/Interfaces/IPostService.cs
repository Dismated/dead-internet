using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetUserPostsAsync(string userId);

        Task<string> GetPostIdAsync(string userId);
    }
}
