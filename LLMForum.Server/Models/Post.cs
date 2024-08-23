namespace LLMForum.Server.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Comment> Comments { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
