using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Exceptions.Base;
using DeadInternet.Server.Interfaces;
using DeadInternet.Server.Models;

namespace DeadInternet.Server.Services
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

        public async Task<Comment> CreatePromptAsync(string promptText, string postId)
        {
            var promptModel = _commentMapper.ToCommentFromCreateDto(promptText, postId, null);
            return await _commentRepo.CreatePromptAsync(promptModel);
        }

        public async Task CreateCommentsAsync(CreateCommentDto createCommentDto)
        {
            var aiComments = await _LLMService.GenerateCommentAsync(createCommentDto.PromptText);
            var savedComments = new List<CommentDto>();
            foreach (var comment in aiComments)
            {
                if (savedComments.Count != 0)
                {
                    createCommentDto.ParentCommentId = savedComments.Last().Id;
                }
                var commentModel = _commentMapper.ToCommentFromCreateDto(
                    comment,
                    createCommentDto.PostId,
                    createCommentDto.ParentCommentId
                );
                await _commentRepo.CreateAsync(commentModel);
                savedComments.Add(_commentMapper.ToCommentDto(commentModel));
            }
        }

        public async Task<List<CommentDto>> GetPostCommentsAsync(string postId)
        {
            var commentDtos = new List<CommentDto>();
            var comments = await _commentRepo.GetPostCommentsAsync(postId);

            foreach (var comment in comments)
            {
                commentDtos.Add(_commentMapper.ToCommentDto(comment));
            }
            return commentDtos;
        }

        public async Task<CommentDto> GetPromptDtoAsync(string postId)
        {
            var prompt =
                await _commentRepo.GetOldestByPostIdAsync(postId)
                ?? throw new NotFoundException("Prompt");
            return _commentMapper.ToCommentDto(prompt);
        }

        public async Task<List<CommentDto>> GetRepliesDtoAsync(string commentId)
        {
            return await _commentRepo.GetRepliesDtoAsync(commentId);
        }

        public async Task<PromptNRepliesDto> GetPromptNRepliesAsync(CommentDto promptDto)
        {
            var replies = await GetRepliesDtoAsync(promptDto.Id);
            return _commentMapper.ToPromptNRepliesDto(promptDto, replies);
        }

        public async Task DeleteCommentChainAsync(string commentId)
        {
            await _commentRepo.DeleteCommentChainAsync(commentId);
        }

        public async Task UpdateCommentAsync(string commentId, string content)
        {
            await _commentRepo.UpdateCommentAsync(commentId, content);
        }

        public async Task<string> CreateReplyAsync(ReplyDto replyDto, string postId)
        {
            var commentModel = _commentMapper.ToCommentFromCreateDto(
                replyDto.Content,
                postId,
                replyDto.ParentCommentId
            );
            await _commentRepo.CreateAsync(commentModel);

            return commentModel.Id;
        }
    }
}
