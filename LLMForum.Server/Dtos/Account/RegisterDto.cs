using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; init; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;

        [Required]
        public string Password { get; init; } = string.Empty
    }
}
