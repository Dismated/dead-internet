using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Dtos.Post;
using LLMForum.Server.Exceptions;
using LLMForum.Server.Interfaces;

namespace LLMForum.Server.Services
{
    public class PostService(
        IPostRepository postRepo,
        IPostMapper postMapper,
        ICommentService commentService
    ) : IPostService
    {
        private readonly IPostRepository _postRepo = postRepo;
        private readonly IPostMapper _postMapper = postMapper;
        private readonly ICommentService _commentService = commentService;

        public async Task<List<PostDto>> GetUserPostsAsync(string userId)
        {
            return await _postRepo.GetUserPostsAsync(userId);
        }

        public async Task<string> GetPostIdAsync(string userId, string prompt)
        {
            var postModel = _postMapper.ToPostFromCreateDto(userId, prompt);
            var post = await _postRepo.CreateAsync(postModel);
            return post.Id;
        }

        public async Task<string> GetPostIdFromCommentAsync(string commentId)
        {
            var comment = await _commentService.GetCommentAsync(commentId);
            return comment.PostId;
        }

        public async Task DeletePostAsync(string postId)
        {
            var postModel =
                await _postRepo.GetByIdAsync(postId) ?? throw new NotFoundException("Post");
            await _postRepo.DeleteAsync(postModel);
        }

        public async Task<PromptNRepliesDto> GetInitialPostPageAsync(string userId, string prompt)
        {
            var postId = await GetPostIdAsync(userId, prompt);
            var promptModel = await _commentService.CreatePromptAsync(prompt, postId);
            var promptDto = _commentService.GetPromptDto(postId);
            await _commentService.CreateCommentsAsync(prompt, postId, promptModel.Id);

            return await _commentService.GetPromptNRepliesAsync(promptDto);
        }

        public async Task<PromptNRepliesDto> GetPostPageAsync(string postId)
        {
            var promptDto = _commentService.GetPromptDto(postId);
            return await _commentService.GetPromptNRepliesAsync(promptDto);
        }
    }
}
