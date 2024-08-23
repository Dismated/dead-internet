using LLMForum.Server.Dtos.Comment;

namespace LLMForum.Server.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentDto>> CreateComments(CreateCommentRequestDto commentDto);
    }
}
