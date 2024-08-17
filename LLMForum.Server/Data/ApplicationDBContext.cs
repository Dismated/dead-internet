using LLMForum.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

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

