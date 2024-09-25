using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface ICommentMapper
    {
        CommentDto ToCommentDto(Comment commentModel);
        Comment ToCommentFromCreateDto(string content, string postId, string? parentCommentId);

        PromptNRepliesDto ToPromptNRepliesDto(CommentDto prompt, List<CommentDto> replies);
    }
}
