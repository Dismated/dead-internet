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

        public static CommentDto GetMockCommentDto()
        {
            return new CommentDto
            {
                Id = GetMockComment().Id,
                Content = GetMockComment().Content,
                CreatedAt = GetMockComment().CreatedAt,
                ParentCommentId = GetMockComment().ParentCommentId,
            };
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

        public static Comment GetMockRandomComment()
        {
            return new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Content = "New Comment",
                CreatedAt = DateTime.Now,
                ParentCommentId = null,
                PostId = Guid.NewGuid().ToString(),
            };
        }

        //Create list of comment strings
        public static List<string> GetMockCommentsList()
        {
            return
            [
                "This is a mock comment.",
                "This is another mock comment.",
                "This is yet another mock comment.",
            ];
        }

        public static CreateCommentRequestDto GetMockCreateCommentRequestDto()
        {
            return new CreateCommentRequestDto
            {
                UserPrompt = "This is a mock user prompt.",
                PostId = "1a",
            };
        }

        public static List<Comment> GetMockCommentModelsList(List<string> commentList)
        {
            var postId = GetMockCreateCommentRequestDto().PostId;
            var list = new List<Comment>();
            foreach (var item in commentList)
            {
                list.Add(
                    new Comment
                    {
                        Id = Guid.NewGuid().ToString(),
                        Content = item,
                        CreatedAt = DateTime.Now,
                        ParentCommentId = list.Count > 0 ? list[^1].Id : null,
                        PostId = postId,
                    }
                );
            }
            return list;
        }

        public static List<CommentDto> GetMockCommentDtosList(List<Comment> commentModelList)
        {
            var list = new List<CommentDto>();

            foreach (var item in commentModelList)
            {
                list.Add(
                    new CommentDto
                    {
                        Id = item.Id,
                        Content = item.Content,
                        CreatedAt = DateTime.Now,
                        ParentCommentId = item.ParentCommentId,
                        PostId = item.PostId,
                    }
                );
            }
            return list;
        }
    }
}
