using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Models;

namespace LLMForum.Tests.MockData
{
    public static class CommentMockData
    {
        public static Comment GetMockComment()
        {
            var mockPost = new Post
            {
                Id = "1a",
                Title = "Sample Post",
                CreatedAt = DateTime.Now.AddMinutes(-60),
            };

            var mockParentComment = new Comment
            {
                Id = "1s",
                Content = "This is a parent comment.",
                CreatedAt = DateTime.Now.AddMinutes(-30),
                PostId = mockPost.Id,
                Post = mockPost,
            };

            var mockComment = new Comment
            {
                Id = "2s",
                Content = "This is a mock comment.",
                CreatedAt = DateTime.Now,
                ParentCommentId = mockParentComment.Id,
                ParentComment = mockParentComment,
                PostId = mockPost.Id,
                Post = mockPost,
            };

            return mockComment;
        }

        public static CreateCommentRequestDto GetMockCommentRequestDto()
        {
            return new CreateCommentRequestDto
            {
                UserPrompt = "This is a mock user prompt.",
                PostId = "1a",
            };
        }

        public static List<CommentDto> GetMockCommentsDtoList()
        {
            return
            [
                new CommentDto
                {
                    Id = "1a",
                    Content = "This is a mock comment.",
                    CreatedAt = DateTime.Now,
                    ParentCommentId = null,
                },
                new CommentDto
                {
                    Id = "2a",
                    Content = "This is a mock comment.",
                    CreatedAt = DateTime.Now,
                    ParentCommentId = "1as",
                },
            ];
        }
    }
}
