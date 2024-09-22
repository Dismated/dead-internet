using System.ComponentModel.DataAnnotations;

namespace DeadInternet.Server.Dtos.Account
{
    public class NewAppUserDto
    {
        [Required]
        public string Username { get; init; } = string.Empty;

        [EmailAddress]
        public string Email { get; init; } = string.Empty;

        [Required]
        public string Token { get; init; } = string.Empty;
    }
}
