﻿namespace LLMForum.Server.Models
{
    public class AIPersonality
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int Votes { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
