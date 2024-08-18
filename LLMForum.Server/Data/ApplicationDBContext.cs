using LLMForum.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LLMForum.Server.Data

{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<AIPersonality> AIPersonalities { get; set; }
    }
}

