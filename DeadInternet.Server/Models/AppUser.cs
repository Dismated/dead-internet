using Microsoft.AspNetCore.Identity;

namespace DeadInternet.Server.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual List<Post> Posts { get; set; } = [];
    }
}
