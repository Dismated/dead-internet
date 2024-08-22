using LLMForum.Server.Dtos.Post;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int id);
        Task<Post> CreateAsync(Post postModel);
        Task<Post?> UpdateAsync(int id, UpdatePostRequestDto postDto);
        Task<Post?> DeleteAsync(int id);
        Task<bool> PostExists(int id);
    }
}
