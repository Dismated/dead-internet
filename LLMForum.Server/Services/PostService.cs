using LLMForum.Server.Interfaces;
using LLMForum.Server.Models;

namespace LLMForum.Server.Services
{
    public class PostService(IPostRepository postRepo, IPostMapper postMapper) : IPostService
    {
        private readonly IPostRepository _postRepo = postRepo;
        private readonly IPostMapper _postMapper = postMapper;

        public async Task<List<Post>> GetUserPostsAsync(string userId)
        {
            var result = await _postRepo.GetUserPostsAsync(userId);
            return result;
        }

        public async Task<string> GetPostIdAsync(string userId)
        {
            var postModel = _postMapper.ToPostFromCreateDto(userId);
            var post = await _postRepo.CreateAsync(postModel);
            return post.Id;
        }
    }
}
