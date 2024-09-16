using LLMForum.Server.Data;
using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LLMForum.Server.Repository
{
    public class CommentRepository(ApplicationDBContext context, ICommentMapper commentMapper)
        : ICommentRepository
    {
        private readonly ApplicationDBContext _context = context;
        private readonly ICommentMapper _commentMapper = commentMapper;

        public async Task<Comment?> GetByIdAsync(string id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<List<Comment>> GetPostCommentsAsync(string postId)
        {
            return await _context.Comments.Where(x => x.PostId == postId).ToListAsync();
        }

        public async Task<Comment> CreatePromptAsync(Comment promptModel)
        {
            await _context.Comments.AddAsync(promptModel);
            await _context.SaveChangesAsync();
            return promptModel;
        }

        public async Task<List<CommentDto>> ReturnThreadAsync(string commentId)
        {
            var comment = await _context
                .Comments.Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment == null)
                return [];

            var commentDto = _commentMapper.ToCommentDto(comment);

            foreach (var reply in commentDto.Replies)
            {
                reply.Replies = await ReturnThreadAsync(reply.Id);
            }

            return [.. commentDto.Replies];
        }
    }
}
