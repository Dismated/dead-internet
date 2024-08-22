namespace LLMForum.Server.Dtos.Post
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}
