namespace LLMForum.Server.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Comment> Comments { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
