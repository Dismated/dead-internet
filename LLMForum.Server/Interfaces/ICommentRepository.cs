using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(string id);
        Task CreateAsync(Comment commentDto);
    }
}
