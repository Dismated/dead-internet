using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Dtos.Post;
using DeadInternet.Server.Models;

namespace DeadInternet.Tests.MockData
{
    public static class PostMockData
    {
        public static Post GetMockPost()
        {
            return new Post
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Sample Post",
                CreatedAt = DateTime.Now,
                Comments = [],
                AppUserId = Guid.NewGuid().ToString(),
            };
        }

        public static List<PostDto> GetMockPostDtoList()
        {
            var list = new List<PostDto>();

            for (int i = 0; i < 2; i++)
            {
                list.Add(
                    new PostDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = $"Sample Post {i}",
                        CreatedAt = DateTime.Now.AddMinutes(-60 * i),
                        Comments = [],
                    }
                );
            }
            return list;
        }

        public static PromptNRepliesDto GetMockPromptNRepliesDto(CommentDto commentDto)
        {
            return new PromptNRepliesDto { Prompt = commentDto, Replies = [] };
        }

        public static List<Post> GetMockPostList()
        {
            var list = new List<Post>();

            for (int i = 0; i < 2; i++)
            {
                list.Add(
                    new Post
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = $"Sample Post {i}",
                        CreatedAt = DateTime.Now,
                        Comments = [],
                        AppUserId = Guid.NewGuid().ToString(),
                    }
                );
            }

            return list;
        }
    }
}
