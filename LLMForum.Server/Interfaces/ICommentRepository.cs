using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Models;
namespace LLMForum.Server.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment commentDto);
    }
}
