using LLMForum.Server.Dtos.Post;
using LLMForum.Server.Models;

namespace LLMForum.Server.Mappers
{
    public static class PostMappers
    {
        public static PostDto ToPostDto(this Post postModel)
        {
            return new PostDto
            {
                Id = postModel.Id,
                Title = postModel.Title,
                CreatedAt = postModel.CreatedAt,
            };
        }

        public static Post ToPostFromCreateDTO(this CreatePostRequestDto postDto)
        {
            return new Post { Title = postDto.Title };
        }
    }
}
