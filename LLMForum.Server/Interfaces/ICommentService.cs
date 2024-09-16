using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> GetCommentAsync(string Id);
        Task<Comment> CreatePromptAsync(string promptText, string postId);
        Task CreateCommentsAsync(string promptText, string postId, string parentCommentId);
        Task<List<CommentDto>> GetPostCommentsAsync(string postId);
        Task<List<CommentDto>> ReturnThreadAsync(string commentId);
    }
}
