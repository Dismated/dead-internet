using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.AppUser
{
    public class UpdateAppUserRequestDto
    {
        [Required]
        public string Username { get; init; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;

        [Required]
        public string PasswordHash { get; init; } = string.Empty;
    }
}
