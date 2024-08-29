using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(string id);
        Task<Comment> CreateAsync(Comment commentDto);
    }
}
