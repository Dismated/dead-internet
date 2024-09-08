using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Interfaces;
using LLMForum.Tests.MockData;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace LLMForum.Tests.Controllers
{
    public class CommentControllerTests
    {
        private readonly ICommentService _commentService;
        private readonly ICommentMapper _commentMapper;
        private readonly CommentController _controller;

        public CommentControllerTests()
        {
            _commentService = Substitute.For<ICommentService>();
            _commentMapper = Substitute.For<ICommentMapper>();
            _controller = new CommentController(_commentService, _commentMapper);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WithCommentDto()
        {
            //Arrange
            string testId = "2s";
            var mockComment = CommentMockData.GetMockComment();
            _commentService.GetCommentAsync(testId).Returns(Task.FromResult(mockComment));
            _commentMapper.ToCommentDto(mockComment).Returns(CommentMockData.GetMockCommentDto());

            //Act
            var result = await _controller.GetById(testId);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<CommentDto>(okResult.Value);
            Assert.Equal(testId, dto.Id);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WithIdAndCommentDtoList()
        {
            //Arrange
            var mockCommentRequestDto = CommentMockData.GetMockCommentRequestDto();
            var mockCommentsDtoList = CommentMockData.GetMockCommentsDtoList();

            _commentService
                .CreateComments(mockCommentRequestDto)
                .Returns(Task.FromResult<List<CommentDto>>(mockCommentsDtoList));

            //Act
            var result = await _controller.Create(mockCommentRequestDto);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtoList = Assert.IsType<List<CommentDto>>(okResult.Value);
            Assert.Equal(mockCommentsDtoList.Count, dtoList.Count);
        }
    }
}
