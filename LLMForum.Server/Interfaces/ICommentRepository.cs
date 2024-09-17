using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace LLMForum.Server.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(string id);
        Task CreateAsync(Comment commentDto);

        Task SaveChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();

        Task<List<Comment>> GetPostCommentsAsync(string postId);

        Task<Comment> CreatePromptAsync(Comment promptModel);

        Task<List<CommentDto>> GetRepliesAsync(string postId);

        Comment? GetOldestByPostId(string postId);
    }
}
