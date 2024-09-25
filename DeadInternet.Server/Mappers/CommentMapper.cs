using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Interfaces;
using DeadInternet.Server.Models;

namespace DeadInternet.Server.Mapper
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
                PostId = commentModel.PostId,
                Replies = commentModel.Replies?.Select(ToCommentDto).ToList() ?? [],
            };
        }

        public Comment ToCommentFromCreateDto(
            string content,
            string postId,
            string? parentCommentId
        )
        {
            return new Comment
            {
                Content = content,
                PostId = postId,
                ParentCommentId = parentCommentId,
            };
        }

        public PromptNRepliesDto ToPromptNRepliesDto(CommentDto prompt, List<CommentDto> replies)
        {
            return new PromptNRepliesDto { Prompt = prompt, Replies = replies };
        }
    }
}
