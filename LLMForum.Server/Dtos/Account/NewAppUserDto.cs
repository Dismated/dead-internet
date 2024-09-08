using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.Account
{
    public class NewAppUserDto
    {
        public string UserName { get; init; } = string.Empty;

        [EmailAddress]
        public string Email { get; init; } = string.Empty;

        [Required]
        public string Token { get; init; } = string.Empty;
    }
}
