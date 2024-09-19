using LLMForum.Server.Dtos.Post;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface IPostRepository
    {
        Task<List<PostDto>> GetUserPostsAsync(string userId);
        Task<Post?> GetByIdAsync(string id);
        Task<Post> CreateAsync(Post postModel);
        Task DeleteAsync(Post postModel);
        Task<bool> PostExists(string id);
    }
}
