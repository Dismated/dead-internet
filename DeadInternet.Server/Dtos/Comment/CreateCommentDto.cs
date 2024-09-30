using System.ComponentModel.DataAnnotations;

namespace DeadInternet.Server.Dtos.Comment
{
    public class CreateCommentDto
    {
        [Required]
        public string PromptText { get; set; } = "";

        [Required]
        public string PostId { get; set; } = "";
        public string? ParentCommentId { get; set; }
    }
}
