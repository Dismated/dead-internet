using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.Comment
{
    public class CreateCommentRequestDto
    {
        [Required]
        public string UserPrompt { get; init; } = String.Empty;

        [Required]
        public string PostId { get; init; } = String.Empty;
    }
}
