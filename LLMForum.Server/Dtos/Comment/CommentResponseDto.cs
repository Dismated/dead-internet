namespace LLMForum.Server.Dtos.Comment
{
    public class CommentResponseDto
    {
        public List<CommentDto> Comments { get; set; }
        public string Message { get; set; }
    }
}
