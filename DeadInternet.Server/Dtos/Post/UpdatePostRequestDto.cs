using System.ComponentModel.DataAnnotations;

namespace DeadInternet.Server.Dtos.Post
{
    public class UpdatePostRequestDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
    }
}
