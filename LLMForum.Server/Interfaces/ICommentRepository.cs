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

        Task<List<CommentDto>> GetRepliesDtoAsync(string postId);

        Comment? GetOldestByPostId(string postId);

        Task<List<Comment>> GetRepliesAsync(string postId);

        Task DeleteCommentChainAsync(string commentId);

        Task CollectCommentsToDelete(string commentId, List<string> commentsToDelete);

        Task DeleteCommentsRecursively(List<string> comments);
    }
}
