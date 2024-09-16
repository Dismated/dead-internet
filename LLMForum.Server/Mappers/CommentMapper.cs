using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Models;

namespace LLMForum.Server.Mapper
{
    public class CommentMapper : ICommentMapper
    {
        public CommentDto ToCommentDto(Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Content = commentModel.Content,
                ParentCommentId = commentModel.ParentCommentId,
                Replies = commentModel.Replies?.Select(ToCommentDto).ToList() ?? [],
            };
        }

        public Comment ToCommentFromCreateDto(
            string comment,
            string postId,
            string? parentCommentId
        )
        {
            return new Comment
            {
                Content = comment,
                PostId = postId,
                ParentCommentId = parentCommentId,
            };
        }
    }
}
