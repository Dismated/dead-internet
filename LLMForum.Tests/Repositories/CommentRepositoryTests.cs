using LLMForum.Server.Data;
using LLMForum.Server.Models;
using LLMForum.Server.Repository;
using LLMForum.Tests.MockData;
using Microsoft.EntityFrameworkCore;

namespace LLMForum.Tests.Repositories
{
    public class CommentRepositoryTests
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly CommentRepository _repository;

        public CommentRepositoryTests()
        {
            _dbContext = GetDbContext().GetAwaiter().GetResult();
            _repository = new CommentRepository(_dbContext);
        }

        private static async Task<ApplicationDBContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new ApplicationDBContext(options);
            dbContext.Database.EnsureCreated();
            if (!await dbContext.Comments.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    dbContext.Comments.Add(
                        new Comment()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Content = $"Comment {i}",
                            CreatedAt = DateTime.Now,
                            ParentCommentId = i == 0 ? Guid.NewGuid().ToString() : null,
                            PostId = Guid.NewGuid().ToString(),
                        }
                    );
                }
                await dbContext.SaveChangesAsync();
            }

            return dbContext;
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsComment_WhenCommentExists()
        {
            //Arrange
            var existingComment = await _dbContext.Comments.FirstAsync();

            //Act
            var comment = await _repository.GetByIdAsync(existingComment.Id);

            //Assert
            Assert.NotNull(comment);
            Assert.Equal(existingComment.Id, comment.Id);
            Assert.Equal(existingComment.Content, comment.Content);
            Assert.Equal(existingComment.CreatedAt, comment.CreatedAt);
            Assert.Equal(existingComment.ParentCommentId, comment.ParentCommentId);
            Assert.Equal(existingComment.PostId, comment.PostId);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenCommentDoesNotExist()
        {
            //Arrange

            //Act
            var comment = await _repository.GetByIdAsync(Guid.NewGuid().ToString());

            //Assert
            Assert.Null(comment);
        }

        [Fact]
        public async Task CreateAsync_ReturnsComment_WhenCommentIsCreated()
        {
            //Arrange
            var mockComment = CommentMockData.GetMockRandomComment();

            //Act
            await _repository.CreateAsync(mockComment);

            //Assert
            var savedComment = await _dbContext.Comments.FirstOrDefaultAsync(c =>
                c.Id == mockComment.Id
            );
            Assert.NotNull(savedComment);
            Assert.Equal("New Comment", savedComment.Content);
        }
    }
}
