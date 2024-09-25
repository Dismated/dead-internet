using System.ComponentModel.DataAnnotations;
using LLMForum.Server.Dtos.Comment;

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
        public List<CommentDto> Comments { get; init; } = [];
    }
}
