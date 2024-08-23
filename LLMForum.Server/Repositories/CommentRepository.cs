using LLMForum.Server.Data;
using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Models;
using Microsoft.SemanticKernel;

namespace LLMForum.Server.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly Kernel _kernel;

        public CommentRepository(ApplicationDBContext context, Kernel kernel)
        {
            _context = context;
            _kernel = kernel;
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);

        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {

            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<string>> GenerateCommentAsync(CreateCommentRequestDto commentModel)
        {
            var changingPrompt = commentModel.UserPrompt;
            List<string> result = [changingPrompt];

            foreach (var num in Enumerable.Range(1, 3))
            {
                var AIResult = await _kernel.InvokePromptAsync<string>(changingPrompt);
                if (AIResult != null)
                {
                    result.Add(AIResult);
                    changingPrompt += $"/n AI Comment: {AIResult}";
                };
            }
            result.RemoveAt(0);
            return result;
        }
    }
}
