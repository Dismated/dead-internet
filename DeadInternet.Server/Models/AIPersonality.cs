namespace DeadInternet.Server.Models
{
    public class AIPersonality
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int Score { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
