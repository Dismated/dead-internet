using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.Comment
{
    public class CommentDto
    {
        [Required]
        public string Id { get; init; } = string.Empty;

        [Required]
        public string Content { get; init; } = string.Empty;

        public DateTime CreatedAt { get; init; }
        public string? ParentCommentId { get; init; }
        public virtual ICollection<CommentDto> Replies { get; set; } = [];

        public string PostId { get; init; } = string.Empty;
    }
}
