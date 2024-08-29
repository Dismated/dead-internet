using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Models;

namespace LLMForum.Server.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
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

        public static Comment ToCommentFromCreateDTO(
            this CreateCommentRequestDto commentDto,
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
