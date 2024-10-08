﻿namespace DeadInternet.Server.Models
{
    public class AIPersonality
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int Votes { get; set; }
        public List<Post> Answers { get; set; }
    }
}
