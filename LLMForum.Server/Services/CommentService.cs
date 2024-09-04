using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Interfaces;

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

        public async Task<List<CommentDto>> CreateComments(CreateCommentRequestDto commentDto)
        {
            var aiComments = await _LLMService.GenerateCommentAsync(commentDto);

            var savedComments = new List<CommentDto>();
            foreach (var comment in aiComments)
            {
                var parentCommentId = savedComments.LastOrDefault()?.Id;
                var commentModel = _commentMapper.ToCommentFromCreateDto(
                    commentDto,
                    comment,
                    parentCommentId
                );
                await _commentRepo.CreateAsync(commentModel);
                savedComments.Add(_commentMapper.ToCommentDto(commentModel));
            }
            return savedComments;
        }
    }
}
