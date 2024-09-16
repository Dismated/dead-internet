using LLMForum.Server.Dtos.Post;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface IPostMapper
    {
        PostDto ToPostDto(Post postModel);
        Post ToPostFromCreateDto(string userId, string prompt);
    }
}
