using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> GetCommentAsync(string Id);
        Task<List<CommentDto>> CreateComments(CreateCommentRequestDto commentDto);
    }
}
