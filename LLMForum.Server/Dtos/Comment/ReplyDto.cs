using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.Comment
{
    public class ReplyDto
    {
        [Required]
        public string Content { get; init; } = String.Empty;

        [Required]
        public string ParentCommentId { get; init; } = String.Empty;
    }
}
