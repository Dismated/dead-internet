using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Models;

namespace DeadInternet.Tests.MockData
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
                Id = Guid.NewGuid().ToString(),
                Content = "This is a mock comment.",
                CreatedAt = DateTime.Now,
                ParentCommentId = Guid.NewGuid().ToString(),
            };
        }

        public static CommentDto GetMockCommentDto(Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Content = commentModel.Content,
                CreatedAt = commentModel.CreatedAt,
                ParentCommentId = commentModel.ParentCommentId,
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

        public static List<string> GetMockCommentsList()
        {
            return
            [
                "This is a mock comment.",
                "This is another mock comment.",
                "This is yet another mock comment.",
            ];
        }

        public static List<Comment> GetMockCommentModelsList()
        {
            return GetMockCommentModelsList(GetMockCommentsList(), Guid.NewGuid().ToString());
        }

        public static List<Comment> GetMockCommentModelsList(
            List<string> commentList,
            string postId
        )
        {
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

        public static CreateCommentDto GetMockCreateCommentDto()
        {
            return new CreateCommentDto
            {
                PromptText = "This is a mock user prompt.",
                PostId = Guid.NewGuid().ToString(),
                ParentCommentId = null,
            };
        }

        public static ReplyDto GetMockReplyDto()
        {
            return new ReplyDto
            {
                Content = "This is a mock reply.",
                ParentCommentId = Guid.NewGuid().ToString(),
            };
        }

        public static Comment GetMockCommentFromReplyDto(ReplyDto replyDto)
        {
            return new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Content = replyDto.Content,
                CreatedAt = DateTime.Now,
                ParentCommentId = replyDto.ParentCommentId,
                PostId = Guid.NewGuid().ToString(),
            };
        }

        public static CreateCommentDto GetMockCreateCommentDto(CommentDto commentDto)
        {
            return new CreateCommentDto
            {
                PromptText = commentDto.Content,
                PostId = commentDto.PostId,
                ParentCommentId = commentDto.ParentCommentId,
            };
        }
    }
}
