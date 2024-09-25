using DeadInternet.Server.Dtos.Post;
using DeadInternet.Server.Models;

namespace DeadInternet.Server.Interfaces
{
    public interface IPostMapper
    {
        PostDto ToPostDto(Post postModel);
        Post ToPostFromCreateDto(string userId, string prompt);
    }
}
