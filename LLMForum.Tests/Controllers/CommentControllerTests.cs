using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Models;
using LLMForum.Tests.MockData;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace LLMForum.Tests.Controllers
{
    public class CommentControllerTests
    {
        private readonly ICommentRepository _commentRepo;
        private readonly ICommentService _commentService;
        private readonly ICommentMapper _commentMapper;
        private readonly CommentController _controller;

        public CommentControllerTests()
        {
            _commentRepo = Substitute.For<ICommentRepository>();
            _commentService = Substitute.For<ICommentService>();
            _commentMapper = Substitute.For<ICommentMapper>();
            _controller = new CommentController(_commentRepo, _commentService, _commentMapper);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WithCommentDto()
        {
            //Arrange
            string testId = "2s";
            var mockComment = CommentMockData.GetMockComment();
            _commentRepo.GetByIdAsync(testId).Returns(Task.FromResult(mockComment));
            _commentMapper.ToCommentDto(mockComment).Returns(CommentMockData.GetMockCommentDto());

            //Act
            var result = await _controller.GetById(testId);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<CommentDto>(okResult.Value);
            Assert.Equal(testId, dto.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenCommentIsNull()
        {
            //Arrange
            string testId = "2i";
            _commentRepo.GetByIdAsync(testId).Returns(Task.FromResult<Comment>(null));

            //Act
            var result = await _controller.GetById(testId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
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
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(nameof(CommentController.GetById), createdAtActionResult.ActionName);

            Assert.Equal(mockCommentsDtoList[0].Id, createdAtActionResult.RouteValues["id"]);

            Assert.NotNull(createdAtActionResult.Value);

            var responseValue = createdAtActionResult.Value;
            var responseType = responseValue.GetType();

            var commentsProperty = responseType.GetProperty("Comments");
            Assert.NotNull(commentsProperty);
            var comments = commentsProperty.GetValue(responseValue) as IEnumerable<CommentDto>;
            Assert.NotNull(comments);
            Assert.Equal(mockCommentsDtoList.Count, comments.Count());

            var messageProperty = responseType.GetProperty("Message");
            Assert.NotNull(messageProperty);
            var message = messageProperty.GetValue(responseValue) as string;
            Assert.Equal($"Successfully created {mockCommentsDtoList.Count} comments", message);

            var commentsList = comments.ToList();
            for (int i = 0; i < mockCommentsDtoList.Count; i++)
            {
                Assert.Equal(mockCommentsDtoList[i].Id, commentsList[i].Id);
                Assert.Equal(mockCommentsDtoList[i].Content, commentsList[i].Content);
                Assert.Equal(mockCommentsDtoList[i].PostId, commentsList[i].PostId);
                Assert.Equal(
                    mockCommentsDtoList[i].ParentCommentId,
                    commentsList[i].ParentCommentId
                );
            }

            await _commentService
                .Received(1)
                .CreateComments(
                    Arg.Is<CreateCommentRequestDto>(dto => dto == mockCommentRequestDto)
                );
        }
    }
}
