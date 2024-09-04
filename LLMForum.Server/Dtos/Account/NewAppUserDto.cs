using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.Account
{
    public class NewAppUserDto
    {
        [Required]
        public string UserName { get; init; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;

        [Required]
        public string Token { get; init; } = string.Empty;
    }
}
