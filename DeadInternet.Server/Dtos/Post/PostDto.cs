using System.ComponentModel.DataAnnotations;
using DeadInternet.Server.Dtos.Comment;

namespace DeadInternet.Server.Dtos.Post
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
