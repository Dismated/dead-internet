using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Dtos.Common;
using DeadInternet.Server.Interfaces;
using DeadInternet.Tests.MockData;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace DeadInternet.Tests.Controllers
{
    public class CommentControllerTests
    {
        private readonly ICommentService _commentService;
        private readonly ICommentMapper _commentMapper;
        private readonly IPostService _postService;
        private readonly CommentController _controller;

        public CommentControllerTests()
        {
            _commentService = Substitute.For<ICommentService>();
            _commentMapper = Substitute.For<ICommentMapper>();
            _postService = Substitute.For<IPostService>();
            _controller = new CommentController(_commentService, _commentMapper, _postService);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WithCommentDto()
        {
            //Arrange
            var mockComment = CommentMockData.GetMockComment();
            var mockCommentDto = CommentMockData.GetMockCommentDto();
            _commentService.GetCommentAsync(mockComment.Id).Returns(Task.FromResult(mockComment));
            _commentMapper.ToCommentDto(mockComment).Returns(mockCommentDto);

            //Act
            var result = await _controller.GetById(mockComment.Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ApiResponse<CommentDto>>(okResult.Value);
            Assert.Equal(mockCommentDto, dto.Data);
        }

        [Fact]
        public async Task GetPromptNRepliesByPostId_ReturnsOk_WithPromptNRepliesDtot()
        {
            //Arrange
            var mockCommentDto = CommentMockData.GetMockCommentDto();
            var mockPromptNRepliesDto = PostMockData.GetMockPromptNRepliesDto(mockCommentDto);
            _postService.GetPostPageAsync(Arg.Any<string>()).Returns(mockPromptNRepliesDto);

            //Act
            var result = await _controller.GetPromptNRepliesByPostId(mockCommentDto.PostId);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ApiResponse<PromptNRepliesDto>>(okResult.Value);
            Assert.Equal(mockPromptNRepliesDto, dto.Data);
        }

        [Fact]
        public async Task DeleteCommentChain_ReturnsOk_WithSuccessMessage()
        {
            //Arrange
            var message = "Post deleted successfully!";
            _commentService.DeleteCommentChainAsync(Arg.Any<string>()).Returns(Task.CompletedTask);

            //Act
            var result = await _controller.DeleteCommentChain("test");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var messageResult = Assert.IsType<MessageResponse>(okResult.Value);
            Assert.Equal(message, messageResult.Message);
        }

        [Fact]
        public async Task UpdateComment_ReturnsOk_WithReplyComments()
        {
            //Arrange
            var mockCommentDto = CommentMockData.GetMockCommentDto();
            var mockCreateCommentDto = CommentMockData.GetMockCreateCommentDto(mockCommentDto);
            _commentService
                .UpdateCommentAsync(mockCommentDto.Id, mockCommentDto.Content)
                .Returns(Task.CompletedTask);
            _postService
                .GetPostIdFromCommentAsync(mockCommentDto.Id)
                .Returns(mockCommentDto.PostId);
            _commentService.CreateCommentsAsync(mockCreateCommentDto).Returns(Task.CompletedTask);
            _commentService.GetRepliesDtoAsync(mockCommentDto.Id).Returns(mockCommentDto.Replies);

            //Act
            var result = await _controller.UpdateComment(
                mockCommentDto.Id,
                new UpdateCommentContentDto { Content = mockCommentDto.Content }
            );

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var okResultDto = Assert.IsType<ApiResponse<List<CommentDto>>>(okResult.Value);
            Assert.Equal(mockCommentDto.Replies, okResultDto.Data);
        }

        [Fact]
        public async Task CreateReply_ReturnsCreatedAtActionResult_WithSuccessMessage()
        {
            //Arrange
            var message = "Reply created successfully!";
            var mockReplyDto = CommentMockData.GetMockReplyDto();
            var mockCommentDto = CommentMockData.GetMockCommentDto();
            var mockCreateCommentDto = CommentMockData.GetMockCreateCommentDto(mockCommentDto);

            _postService
                .GetPostIdFromCommentAsync(mockReplyDto.ParentCommentId)
                .Returns(Task.FromResult(mockCommentDto.PostId));
            _commentService
                .CreateReplyAsync(mockReplyDto, mockCommentDto.PostId)
                .Returns(Task.FromResult(mockCommentDto.Id));
            _commentService.CreateCommentsAsync(mockCreateCommentDto).Returns(Task.CompletedTask);

            //Act
            var result = await _controller.CreateReply(mockReplyDto);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var messageResult = Assert.IsType<MessageResponse>(okResult.Value);
            Assert.Equal(message, messageResult.Message);
        }
    }
}
