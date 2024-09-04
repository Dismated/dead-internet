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
                CreatedAt = commentModel.CreatedAt,
                PostId = commentModel.PostId,
                ParentCommentId = commentModel.ParentCommentId,
            };
        }

        public Comment ToCommentFromCreateDto(
            CreateCommentRequestDto commentDto,
            string comment,
            string? parentCommentId
        )
        {
            return new Comment
            {
                Content = comment,
                PostId = commentDto.PostId,
                ParentCommentId = parentCommentId,
            };
        }
    }
}
