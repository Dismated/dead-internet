using DeadInternet.Server.Dtos.Post;
using DeadInternet.Server.Models;

namespace DeadInternet.Server.Interfaces
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
