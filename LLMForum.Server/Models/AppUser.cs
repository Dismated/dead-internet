using Microsoft.AspNetCore.Identity;

namespace LLMForum.Server.Models
{
    public class AppUser : IdentityUser
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Post> Posts { get; set; }
    }
}

