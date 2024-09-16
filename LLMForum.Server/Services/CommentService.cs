using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Exceptions;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Models;

namespace LLMForum.Server.Services
{
    public class CommentService(
        ICommentRepository commentRepo,
        ILLMService LLMService,
        ICommentMapper commentMapper
    ) : ICommentService
    {
        private readonly ICommentRepository _commentRepo = commentRepo;
        private readonly ILLMService _LLMService = LLMService;
        private readonly ICommentMapper _commentMapper = commentMapper;

        public async Task<Comment> GetCommentAsync(string id)
        {
            var result = await _commentRepo.GetByIdAsync(id);
            return result ?? throw new NotFoundException("Comment");
        }

        public async Task<List<CommentDto>> CreateComments(string promptText, string postId)
        {
            var aiComments = await _LLMService.GenerateCommentAsync(promptText);
            var savedComments = new List<CommentDto>();

            foreach (var comment in aiComments)
            {
                string? parentCommentId = null;
                if (savedComments.Count != 0)
                {
                    Console.WriteLine($"1{savedComments[0].Id}");
                    Console.WriteLine($"1{savedComments[0].Content}");

                    Console.WriteLine($"2{savedComments[0]}");

                    parentCommentId = savedComments.Last().Id;
                }
                Console.WriteLine($"3{savedComments}");
                Console.WriteLine(parentCommentId ?? "null");
                var commentModel = _commentMapper.ToCommentFromCreateDto(
                    comment,
                    postId,
                    parentCommentId
                );

                try
                {
                    await _commentRepo.CreateAsync(commentModel);
                    savedComments.Add(_commentMapper.ToCommentDto(commentModel));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving comment: {ex.Message}");
                    throw;
                }
            }
            return savedComments;
        }
    }
}
