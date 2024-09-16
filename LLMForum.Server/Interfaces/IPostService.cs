using LLMForum.Server.Dtos.Post;

namespace LLMForum.Server.Interfaces
{
    public interface IPostService
    {
        Task<List<PostDto>> GetUserPostsAsync(string userId);

        Task<string> GetPostIdAsync(string userId, string prompt);

        Task DeletePostAsync(string postId);
    }
}
