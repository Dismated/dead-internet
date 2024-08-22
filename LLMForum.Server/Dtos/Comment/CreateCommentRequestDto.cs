namespace LLMForum.Server.Dtos.Comment
{
    public class CreateCommentRequestDto
    {
        public string UserPrompt { get; set; }

        public int PostId { get; set; }

    }

}
