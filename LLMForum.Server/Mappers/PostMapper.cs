using LLMForum.Server.Dtos.Post;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Models;

namespace LLMForum.Server.Mappers
{
    public class PostMapper : IPostMapper
    {
        public PostDto ToPostDto(Post postModel)
        {
            return new PostDto
            {
                Id = postModel.Id,
                Title = postModel.Title,
                CreatedAt = postModel.CreatedAt,
            };
        }

        public Post ToPostFromCreateDto(string userId)
        {
            return new Post { AppUserId = userId };
        }
    }
}
