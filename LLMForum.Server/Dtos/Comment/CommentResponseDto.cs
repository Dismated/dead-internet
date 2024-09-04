using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.Comment
{
    public class CommentResponseDto
    {
        [Required]
        public List<CommentDto> Comments { get; init; } = new List<CommentDto>();

        [Required]
        public string Message { get; init; } = string.Empty;
    }
}
