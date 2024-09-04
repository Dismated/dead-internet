using Microsoft.AspNetCore.Identity;

namespace LLMForum.Server.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual List<Post> Posts { get; set; } = new List<Post>();
    }
}
