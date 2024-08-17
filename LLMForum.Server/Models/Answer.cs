namespace LLMForum.Server.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int PersonalityId { get; set; }
        public AIPersonality Personality { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
