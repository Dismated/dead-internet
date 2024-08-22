using LLMForum.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LLMForum.Server.Data

{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<AIPersonality> AIPersonalities { get; set; }
    }
}

