using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Mappers;

namespace LLMForum.Server.Services
{
    public class CommentService(ICommentRepository commentRepo, ILLMService LLMService) : ICommentService
    {
        private readonly ICommentRepository _commentRepo = commentRepo;
        private readonly ILLMService _LLMService = LLMService;

        public async Task<List<CommentDto>> CreateComments(CreateCommentRequestDto commentDto)
        {
            var aiComments = await _LLMService.GenerateCommentAsync(commentDto);

            var savedComments = new List<CommentDto>();
            foreach (var comment in aiComments)
            {
                var parentCommentId = savedComments.LastOrDefault()?.Id;
                var commentModel = commentDto.ToCommentFromCreateDTO(comment, parentCommentId);
                await _commentRepo.CreateAsync(commentModel);
                savedComments.Add(commentModel.ToCommentDto());
            }
            return savedComments;
        }
    }
}
