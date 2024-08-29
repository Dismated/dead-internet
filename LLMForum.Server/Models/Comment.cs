namespace LLMForum.Server.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ParentCommentId { get; set; }
        public virtual Comment ParentComment { get; set; }
        public string PostId { get; set; }
        public Post Post { get; set; }
    }
}
