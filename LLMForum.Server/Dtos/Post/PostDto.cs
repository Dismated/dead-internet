namespace LLMForum.Server.Dtos.Post
{
    public class PostDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
    }
}
