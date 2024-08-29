namespace LLMForum.Server.Dtos.Comment
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ParentCommentId { get; set; }
        public string PostId { get; set; }
    }
}
