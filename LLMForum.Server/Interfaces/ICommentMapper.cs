using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface ICommentMapper
    {
        CommentDto ToCommentDto(Comment commentModel);
        Comment ToCommentFromCreateDto(
            CreateCommentRequestDto commentDto,
            string comment,
            string? parentCommentId
        );
    }
}
