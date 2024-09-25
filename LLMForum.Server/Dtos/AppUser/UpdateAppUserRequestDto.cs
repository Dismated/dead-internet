using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.AppUser
{
    public class UpdateAppUserRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;

        [Required]
        public string PasswordHash { get; init; } = string.Empty;
    }
}
