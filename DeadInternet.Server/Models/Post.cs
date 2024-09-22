using System.ComponentModel.DataAnnotations;

namespace DeadInternet.Server.Models
{
    public class Post
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<Comment> Comments { get; set; } = [];

        [Required]
        public string AppUserId { get; set; } = string.Empty;

        [Required]
        public AppUser AppUser { get; set; } = null!;
    }
}
