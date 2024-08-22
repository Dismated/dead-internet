namespace LLMForum.Server.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? ParentCommentId { get; set; }
        public virtual Comment ParentComment { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
