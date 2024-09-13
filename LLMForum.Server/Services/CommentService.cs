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
                var parentCommentId = savedComments.LastOrDefault()?.Id;
                var commentModel = _commentMapper.ToCommentFromCreateDto(
                    comment,
                    postId,
                    parentCommentId
                );
                await _commentRepo.CreateAsync(commentModel);
                savedComments.Add(_commentMapper.ToCommentDto(commentModel));
            }
            return savedComments;
        }
    }
}
