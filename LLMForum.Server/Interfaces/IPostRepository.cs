using LLMForum.Server.Dtos.Post;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(string id);
        Task<Post> CreateAsync(Post postModel);
        Task<Post?> UpdateAsync(string id, UpdatePostRequestDto postDto);
        Task<Post?> DeleteAsync(string id);
        Task<bool> PostExists(string id);
    }
}
