using LLMForum.Server.Models;

namespace LLMForum.Server.Dtos.AppUser
{
    public class AppUserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
