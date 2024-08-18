using LLMForum.Server.Models;

namespace LLMForum.Server.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
