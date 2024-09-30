using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Dtos.Post;
using DeadInternet.Server.Exceptions.Base;
using DeadInternet.Server.Interfaces;
using DeadInternet.Server.Models;
using DeadInternet.Server.Services;
using DeadInternet.Tests.MockData;
using NSubstitute;

namespace DeadInternet.Tests.Services
{
    public class PostServiceTests
    {
        private readonly IPostRepository _postRepo;
        private readonly IPostMapper _postMapper;
        private readonly ICommentService _commentService;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            _postRepo = Substitute.For<IPostRepository>();
            _postMapper = Substitute.For<IPostMapper>();
            _commentService = Substitute.For<ICommentService>();

            _postService = new PostService(_postRepo, _postMapper, _commentService);
        }

        [Fact]
        public async Task GetUserPostsAsync_ReturnsValidPostList()
        {
            //Arrange
            var mockPostDtoList = PostMockData.GetMockPostDtoList();
            _postRepo
                .GetUserPostsAsync(Arg.Any<string>())
                .Returns(Task.FromResult(mockPostDtoList));

            //Act
            var result = await _postService.GetUserPostsAsync("userId");

            //Assert
            Assert.IsType<List<PostDto>>(result);
        }

        [Fact]
        public async Task GetPostIdAsync_ReturnsValisPostId()
        {
            //Arrange
            var mockPost = PostMockData.GetMockPost();
            _postMapper.ToPostFromCreateDto(mockPost.AppUserId, mockPost.Title).Returns(mockPost);
            _postRepo.CreateAsync(mockPost).Returns(Task.FromResult(mockPost));

            //Act
            var result = await _postService.GetPostIdAsync(mockPost.AppUserId, mockPost.Title);

            //Assert
            Assert.IsType<string>(result);
            Assert.Equal(mockPost.Id, result);
        }

        [Fact]
        public async Task GetPostIdFromCommentAsync_ReturnsValidPostId()
        {
            //Arrange
            var mockComment = CommentMockData.GetMockComment();
            _commentService.GetCommentAsync(mockComment.Id).Returns(Task.FromResult(mockComment));

            //Act
            var result = await _postService.GetPostIdFromCommentAsync(mockComment.Id);

            //Assert
            Assert.Equal(mockComment.PostId, result);
        }

        [Fact]
        public async Task DeletePostAsync_PostNotFound_ThrowsNotFoundException()
        {
            //Arrange
            _postRepo.GetByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<Post?>(null));

            //Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _postService.DeletePostAsync("postId")
            );
        }

        [Fact]
        public async Task DeletePostAsync_DeletesPost()
        {
            //Arrange
            var mockPost = PostMockData.GetMockPost();
            _postRepo.GetByIdAsync(mockPost.Id).Returns(Task.FromResult<Post?>(mockPost));
            _postRepo.DeleteAsync(mockPost).Returns(Task.CompletedTask);

            //Act
            await _postService.DeletePostAsync(mockPost.Id);

            //Assert
            await _postRepo.Received(1).DeleteAsync(mockPost);
        }

        [Fact]
        public async Task GetInitialPostPageAsync_ReturnsPromptNRepliesDto()
        {
            var mockPost = PostMockData.GetMockPost();
            var mockComment = CommentMockData.GetMockComment();
            var mockCommentDto = CommentMockData.GetMockCommentDto(mockComment);
            var mockPromptNRepliesDto = PostMockData.GetMockPromptNRepliesDto(mockCommentDto);

            _postMapper.ToPostFromCreateDto(mockPost.AppUserId, mockPost.Title).Returns(mockPost);
            _postRepo.CreateAsync(mockPost).Returns(mockPost);
            _commentService.CreatePromptAsync(mockPost.Title, mockPost.Id).Returns(mockComment);
            _commentService.GetPromptDtoAsync(Arg.Any<string>()).Returns(mockCommentDto);
            _commentService
                .CreateCommentsAsync(Arg.Any<CreateCommentDto>())
                .Returns(Task.CompletedTask);

            _commentService
                .GetPromptNRepliesAsync(mockCommentDto)
                .Returns(Task.FromResult(mockPromptNRepliesDto));

            // Act
            var result = await _postService.GetInitialPostPageAsync(
                mockPost.AppUserId,
                mockPost.Title
            );

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PromptNRepliesDto>(result);
            Assert.Equal(mockPromptNRepliesDto, result);
        }

        [Fact]
        public async Task GetPostPageAsync_ReturnsPromptNRepliesDto()
        {
            //Arrange
            var mockCommentDto = CommentMockData.GetMockCommentDto();
            var mockPromptNRepliesDto = PostMockData.GetMockPromptNRepliesDto(mockCommentDto);

            _commentService.GetPromptDtoAsync(mockCommentDto.PostId).Returns(mockCommentDto);
            _commentService.GetPromptNRepliesAsync(mockCommentDto).Returns(mockPromptNRepliesDto);

            //Act
            var result = await _postService.GetPostPageAsync(mockCommentDto.PostId);

            //Assert
            Assert.IsType<PromptNRepliesDto>(result);
            Assert.Equal(mockPromptNRepliesDto, result);
        }
    }
}
