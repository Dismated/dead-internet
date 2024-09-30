using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Exceptions.Base;
using DeadInternet.Server.Interfaces;
using DeadInternet.Server.Models;
using DeadInternet.Server.Services;
using DeadInternet.Tests.MockData;
using NSubstitute;

namespace DeadInternet.Tests.Services
{
    public class CommentServiceTests
    {
        private readonly ICommentRepository _commentRepo;
        private readonly ILLMService _LLMService;
        private readonly ICommentMapper _commentMapper;
        private readonly ICommentService _commentService;

        public CommentServiceTests()
        {
            _commentRepo = Substitute.For<ICommentRepository>();
            _LLMService = Substitute.For<ILLMService>();
            _commentMapper = Substitute.For<ICommentMapper>();
            _commentService = new CommentService(_commentRepo, _LLMService, _commentMapper);
        }

        [Fact]
        public async Task GetCommentAsync_CommentNotFound_ThrowsNotFoundException()
        {
            //Arrange
            _commentRepo.GetByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<Comment?>(null));

            //Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _commentService.GetCommentAsync("commentId")
            );
        }

        [Fact]
        public async Task GetCommentAsync_ReturnsValidComment()
        {
            //Arrange
            var mockRandomComment = CommentMockData.GetMockRandomComment();

            _commentRepo
                .GetByIdAsync(mockRandomComment.Id)
                .Returns(Task.FromResult<Comment?>(mockRandomComment));

            //Act
            var result = await _commentService.GetCommentAsync(mockRandomComment.Id);

            //Assert
            Assert.Equal(mockRandomComment, result);
        }

        [Fact]
        public async Task CreateCommentsAsync_ReturnsComments()
        {
            //Arrange
            var mockCreateCommentDto = CommentMockData.GetMockCreateCommentDto();
            var mockCommentList = CommentMockData.GetMockCommentsList();
            var mockCommentModelsList = CommentMockData.GetMockCommentModelsList(
                mockCommentList,
                mockCreateCommentDto.PostId
            );
            var mockCommentDtosList = CommentMockData.GetMockCommentDtosList(mockCommentModelsList);

            _LLMService
                .GenerateCommentAsync(Arg.Any<string>())
                .Returns(Task.FromResult(mockCommentList));

            for (int i = 0; i < mockCommentList.Count; i++)
            {
                var mockParentId = i == 0 ? null : mockCommentModelsList[i - 1].Id;

                _commentMapper
                    .ToCommentFromCreateDto(
                        mockCommentList[i],
                        mockCreateCommentDto.PostId,
                        Arg.Any<string?>()
                    )
                    .Returns(mockCommentModelsList[i]);

                _commentRepo.CreateAsync(mockCommentModelsList[i]).Returns(Task.CompletedTask);

                _commentMapper
                    .ToCommentDto(mockCommentModelsList[i])
                    .Returns(mockCommentDtosList[i]);
            }

            //Act
            await _commentService.CreateCommentsAsync(mockCreateCommentDto);

            //Assert
            for (int i = 0; i < mockCommentList.Count; i++)
            {
                var mockParentId = i == 0 ? null : mockCommentModelsList[i - 1].Id;

                _commentMapper
                    .Received(1)
                    .ToCommentFromCreateDto(
                        mockCommentList[i],
                        mockCreateCommentDto.PostId,
                        mockParentId
                    );
                await _commentRepo.Received(1).CreateAsync(mockCommentModelsList[i]);
                _commentMapper.Received(1).ToCommentDto(mockCommentModelsList[i]);
            }
        }

        [Fact]
        public async Task GetPostCommentsAsync_ReturnsAllComments()
        {
            //Arrange
            var mockCommentsList = CommentMockData.GetMockCommentModelsList();
            var mockCommentDtosList = CommentMockData.GetMockCommentDtosList(mockCommentsList);

            _commentRepo
                .GetPostCommentsAsync(mockCommentsList[0].PostId)
                .Returns(Task.FromResult(mockCommentsList));

            for (int i = 0; i < mockCommentsList.Count; i++)
            {
                _commentMapper.ToCommentDto(mockCommentsList[i]).Returns(mockCommentDtosList[i]);
            }

            //Act
            var result = await _commentService.GetPostCommentsAsync(mockCommentsList[0].PostId);

            //Assert
            Assert.IsType<List<CommentDto>>(result);
            Assert.Equal(mockCommentDtosList, result);
        }

        [Fact]
        public async Task GetPromptDto_PromptNotFound_ThrowsNotFoundException()
        {
            //Arrange
            _commentRepo
                .GetOldestByPostIdAsync(Arg.Any<string>())
                .Returns(Task.FromResult<Comment?>(null));

            //Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                async () => await _commentService.GetPromptDtoAsync(Arg.Any<string>())
            );
        }

        [Fact]
        public async Task GetPromptDto_ReturnsValidPromptDto()
        {
            //Arrange
            var mockComment = CommentMockData.GetMockComment();
            var mockCommentDto = CommentMockData.GetMockCommentDto(mockComment);

            _commentRepo
                .GetOldestByPostIdAsync(mockComment.PostId)
                .Returns(Task.FromResult<Comment?>(mockComment));
            _commentMapper.ToCommentDto(mockComment).Returns(mockCommentDto);

            //Act
            var result = await _commentService.GetPromptDtoAsync(mockComment.PostId);

            //Assert
            Assert.Equal(mockCommentDto, result);
        }

        [Fact]
        public async Task GetRepliesDtoAsync_ReturnsCommentDtoList()
        {
            //Arrange
            var mockCommentId = Guid.NewGuid().ToString();
            var mockCommentsList = CommentMockData.GetMockCommentModelsList();
            var mockCommentDtosList = CommentMockData.GetMockCommentDtosList(mockCommentsList);
            _commentRepo
                .GetRepliesDtoAsync(mockCommentId)
                .Returns(Task.FromResult(mockCommentDtosList));

            //Act
            var result = await _commentService.GetRepliesDtoAsync(mockCommentId);

            //Assert
            Assert.IsType<List<CommentDto>>(result);
        }

        [Fact]
        public async Task DeleteCommentChainAsync_DeletesAllReplies()
        {
            //Arrange
            var commentId = Guid.NewGuid().ToString();
            _commentRepo.DeleteCommentChainAsync(commentId).Returns(Task.CompletedTask);

            //Act
            await _commentService.DeleteCommentChainAsync(commentId);

            //Assert
            await _commentRepo.Received(1).DeleteCommentChainAsync(commentId);
        }

        [Fact]
        public async Task UpdatecommentAsync_UpdatesComment()
        {
            //Arrange
            var mockCommentId = Guid.NewGuid().ToString();
            var mockContent = "new content";

            _commentRepo.UpdateCommentAsync(mockCommentId, mockContent).Returns(Task.CompletedTask);

            //Act
            await _commentService.UpdateCommentAsync(mockCommentId, mockContent);

            //Assert
            await _commentRepo.Received(1).UpdateCommentAsync(mockCommentId, mockContent);
        }

        [Fact]
        public async Task CreateReplyAsync_ReturnsCommentId()
        {
            //Arrange
            var mockReplyDto = CommentMockData.GetMockReplyDto();
            var mockCommentFromReplyDto = CommentMockData.GetMockCommentFromReplyDto(mockReplyDto);
            _commentMapper
                .ToCommentFromCreateDto(
                    mockReplyDto.Content,
                    mockCommentFromReplyDto.PostId,
                    mockReplyDto.ParentCommentId
                )
                .Returns(mockCommentFromReplyDto);
            _commentRepo.CreateAsync(mockCommentFromReplyDto).Returns(Task.CompletedTask);

            //Act
            var result = await _commentService.CreateReplyAsync(
                mockReplyDto,
                mockCommentFromReplyDto.PostId
            );

            //Assert
            Assert.Equal(mockCommentFromReplyDto.Id, result);
        }
    }
}
