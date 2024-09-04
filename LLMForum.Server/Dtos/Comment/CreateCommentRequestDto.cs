namespace LLMForum.Server.Dtos.Comment
{
    public class CreateCommentRequestDto
    {
        public string UserPrompt { get; set; } = String.Empty;

        public string PostId { get; set; } = String.Empty;
    }
}
