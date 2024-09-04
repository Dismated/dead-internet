using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.Post
{
    public class PostDto
    {
        [Required]
        public string Id { get; init; } = string.Empty;

        [Required]
        public string Title { get; init; } = string.Empty;

        public DateTime CreatedAt { get; init; }

        [Required]
        public string UserId { get; init; } = string.Empty;
    }
}
