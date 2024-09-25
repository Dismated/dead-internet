using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.Comment
{
    public class PromptNRepliesDto
    {
        [Required]
        public CommentDto Prompt { get; set; } = null!;

        [Required]
        public List<CommentDto> Replies { get; set; } = [];
    }
}
