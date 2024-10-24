using System.Security.Claims;
using DeadInternet.Server.Controllers;
using DeadInternet.Server.Dtos.AppUser;
using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Dtos.Common;
using DeadInternet.Server.Dtos.Post;
using DeadInternet.Server.Exceptions.Base;
using DeadInternet.Server.Interfaces;
using DeadInternet.Tests.MockData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace DeadInternet.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly IPostService _postService;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _postService = Substitute.For<IPostService>();
            _controller = new HomeController(_postService);
        }

        [Fact]
        public async Task GetUserPosts_UserNotFound_ThrowsNotFoundException()
        {
            //Arrange
            var userId = "testUserId";
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user },
            };

            //Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetUserPosts());
        }

        [Fact]
        public async Task GetUserPosts_ReturnsOk_WithAllUserPosts()
        {
            //Arrange
            var userId = "testUserId";
            var user = new ClaimsPrincipal(
                new ClaimsIdentity([new(ClaimTypes.NameIdentifier, userId)])
            );
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user },
            };

            var mockPostDtoList = PostMockData.GetMockPostDtoList();

            _postService.GetUserPostsAsync(userId).Returns(Task.FromResult(mockPostDtoList));

            //Act
            var result = await _controller.GetUserPosts();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var okResultValue = Assert.IsType<ApiResponse<List<PostDto>>>(okResult.Value);
            Assert.Equal(mockPostDtoList, okResultValue.Data);
        }

        [Fact]
        public async Task CreateLLMResponse_UserNotFound_ThrowsNotFoundException()
        {
            //Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user },
            };

            var mockAppUserPromptDto = new AppUserPromptDto { Prompt = "testPrompt" };

            //Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _controller.CreateLLMResponse(mockAppUserPromptDto)
            );
        }

        [Fact]
        public async Task CreateLLMResponse_ReturnsOk_WithPromptNRepliesDto()
        {
            //Arrange
            var userId = "testUserId";
            var user = new ClaimsPrincipal(
                new ClaimsIdentity([new(ClaimTypes.NameIdentifier, userId)])
            );
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user },
            };

            var mockAppUserPromptDto = new AppUserPromptDto { Prompt = "testPrompt" };
            var mockCommentDto = CommentMockData.GetMockCommentDto();
            var mockPromptNRepliesDto = PostMockData.GetMockPromptNRepliesDto(mockCommentDto);

            _postService
                .GetInitialPostPageAsync(userId, Arg.Any<string>())
                .Returns(Task.FromResult(mockPromptNRepliesDto));

            //Act
            var result = await _controller.CreateLLMResponse(mockAppUserPromptDto);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var okResultValue = Assert.IsType<ApiResponse<PromptNRepliesDto>>(okResult.Value);
            Assert.Equal(mockPromptNRepliesDto, okResultValue.Data);
        }

        [Fact]
        public async Task DeletePost_ReturnsOk_WithSuccessMessage()
        {
            //Arrange
            _postService.DeletePostAsync(Arg.Any<string>()).Returns(Task.CompletedTask);

            //Act
            var result = await _controller.DeletePost("testPostId");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var messageResult = Assert.IsType<MessageResponse>(okResult.Value);
            Assert.Equal("Post deleted successfully!", messageResult.Message);
        }
    }
}
