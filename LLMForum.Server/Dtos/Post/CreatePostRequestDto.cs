using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.Post
{
    public class CreatePostRequestDto
    {
        [Required]
        public string Title { get; init; } = string.Empty;
    }
}
