using DeadInternet.Server.Data;
using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Dtos.Post;
using DeadInternet.Server.Interfaces;
using DeadInternet.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DeadInternet.Server.Repository
{
    public class PostRepository(ApplicationDBContext context) : IPostRepository
    {
        private readonly ApplicationDBContext _context = context;

        public async Task<List<PostDto>> GetUserPostsAsync(string userId)
        {
            return await _context
                .Posts.Where(x => x.AppUserId == userId)
                .OrderByDescending(post => post.CreatedAt)
                .Select(post => new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Comments = post
                        .Comments.Select(comment => new CommentDto
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            CreatedAt = comment.CreatedAt,
                        })
                        .ToList(),
                })
                .ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(string id)
        {
            return await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Post> CreateAsync(Post postModel)
        {
            await _context.Posts.AddAsync(postModel);
            await _context.SaveChangesAsync();
            return postModel;
        }

        public async Task DeleteAsync(Post postModel)
        {
            _context.Posts.Remove(postModel);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> PostExists(string id)
        {
            return await _context.Posts.AnyAsync(x => x.Id == id);
        }
    }
}
