using LLMForum.Server.Dtos.Post;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllAsync();
        Task<List<Post>> GetUserPostsAsync(string userId);
        Task<Post?> GetByIdAsync(string id);
        Task<Post> CreateAsync(Post postModel);
        Task<Post?> UpdateAsync(string id, UpdatePostRequestDto postDto);
        Task DeleteAsync(Post postModel);
        Task<bool> PostExists(string id);
    }
}
