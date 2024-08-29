namespace LLMForum.Server.Dtos.Comment
{
    public class CreateCommentRequestDto
    {
        public string UserPrompt { get; set; }

        public string PostId { get; set; }
    }
}
