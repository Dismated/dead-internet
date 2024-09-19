using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.Comment
{
    public class UpdateCommentContentDto
    {
        [Required]
        public string Content { get; init; } = string.Empty;
    }
}
