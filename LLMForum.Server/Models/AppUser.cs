using Microsoft.AspNetCore.Identity;

namespace LLMForum.Server.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public List<Post> Posts { get; set; }
    }
}
