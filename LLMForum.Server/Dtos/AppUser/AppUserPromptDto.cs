using System.ComponentModel.DataAnnotations;

namespace LLMForum.Server.Dtos.AppUser
{
    public class AppUserPromptDto
    {
        [Required]
        public string Prompt { get; set; } = String.Empty;
    }
}
