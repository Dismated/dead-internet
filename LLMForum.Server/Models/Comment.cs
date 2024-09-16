using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Models
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ParentCommentId { get; set; }
        public virtual Comment? ParentComment { get; set; } = null!;

        public virtual ICollection<Comment> Replies { get; set; } = [];

        [Required]
        public string PostId { get; set; } = string.Empty;

        [Required]
        public virtual Post Post { get; set; } = null!;
    }
}
