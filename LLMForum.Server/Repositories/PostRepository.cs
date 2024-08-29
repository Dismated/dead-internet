using LLMForum.Server.Data;
using LLMForum.Server.Dtos.Post;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LLMForum.Server.Repository
{
    public class PostRepository(ApplicationDBContext context) : IPostRepository
    {
        private readonly ApplicationDBContext _context = context;

        public async Task<List<Post>> GetAllAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(string id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<Post?> UpdateAsync(string id, UpdatePostRequestDto postDto)
        {
            var existingPost = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (existingPost == null)
            {
                return null;
            }
            existingPost.Title = postDto.Title;

            await _context.SaveChangesAsync();
            return existingPost;
        }

        public async Task<Post> CreateAsync(Post postModel)
        {
            await _context.Posts.AddAsync(postModel);
            await _context.SaveChangesAsync();
            return postModel;
        }

        public async Task<Post?> DeleteAsync(string id)
        {
            var postModel = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if (postModel == null)
            {
                return null;
            }
            _context.Posts.Remove(postModel);
            await _context.SaveChangesAsync();
            return postModel;
        }

        public async Task<bool> PostExists(string id)
        {
            return await _context.Posts.AnyAsync(x => x.Id == id);
        }
    }
}
