using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Models;

namespace DeadInternet.Server.Interfaces
{
    public interface ICommentMapper
    {
        CommentDto ToCommentDto(Comment commentModel);
        Comment ToCommentFromCreateDto(string content, string postId, string? parentCommentId);

        PromptNRepliesDto ToPromptNRepliesDto(CommentDto prompt, List<CommentDto> replies);
    }
}
