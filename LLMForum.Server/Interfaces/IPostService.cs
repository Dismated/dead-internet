using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Dtos.Post;

namespace LLMForum.Server.Interfaces
{
    public interface IPostService
    {
        Task<List<PostDto>> GetUserPostsAsync(string userId);

        Task<string> GetPostIdAsync(string userId, string prompt);

        Task<string> GetPostIdFromCommentAsync(string commentId);

        Task DeletePostAsync(string postId);

        Task<PromptNRepliesDto> GetInitialPostPageAsync(string userId, string prompt);
        Task<PromptNRepliesDto> GetPostPageAsync(string postId);
    }
}
