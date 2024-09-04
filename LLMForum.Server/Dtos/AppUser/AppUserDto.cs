using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.AppUser
{
    public class AppUserDto
    {
        [Required]
        public string Id { get; init; } = string.Empty;

        [Required]
        public string Username { get; init; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}
