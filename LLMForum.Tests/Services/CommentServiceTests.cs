using LLMForum.Server.Interfaces;
using LLMForum.Server.Services;
using LLMForum.Tests.MockData;
using NSubstitute;

namespace LLMForum.Tests.Services
{
    public class CommentServiceTests
    {
        [Fact]
        public async Task CreateComments_ReturnsComments()
        {
            //Arrange
            var commentRepo = Substitute.For<ICommentRepository>();
            var LLMService = Substitute.For<ILLMService>();
            var commentMapper = Substitute.For<ICommentMapper>();
            var commentService = new CommentService(commentRepo, LLMService, commentMapper);

            var mockCreateCommentRequestDto = CommentMockData.GetMockCreateCommentRequestDto();
            var mockCommentList = CommentMockData.GetMockCommentsList();
            var mockCommentModelsList = CommentMockData.GetMockCommentModelsList(mockCommentList);
            var mockCommentDtosList = CommentMockData.GetMockCommentDtosList(mockCommentModelsList);

            LLMService
                .GenerateCommentAsync(mockCreateCommentRequestDto)
                .Returns(Task.FromResult(mockCommentList));

            for (int i = 0; i < mockCommentList.Count; i++)
            {
                commentMapper
                    .ToCommentFromCreateDto(
                        mockCreateCommentRequestDto,
                        mockCommentList[i],
                        i == 0 ? null : mockCommentModelsList[i - 1].Id
                    )
                    .Returns(mockCommentModelsList[i]);

                commentRepo
                    .CreateAsync(mockCommentModelsList[i])
                    .Returns(Task.FromResult(Task.CompletedTask));

                commentMapper
                    .ToCommentDto(mockCommentModelsList[i])
                    .Returns(mockCommentDtosList[i]);
            }

            //Act
            var result = await commentService.CreateComments(mockCreateCommentRequestDto);

            //Assert
            Assert.Equal(3, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(mockCommentDtosList[i].Content, result[i].Content);
                Assert.Equal(mockCommentDtosList[i].ParentCommentId, result[i].ParentCommentId);

                commentMapper
                    .Received(1)
                    .ToCommentFromCreateDto(
                        mockCreateCommentRequestDto,
                        mockCommentList[i],
                        i == 0 ? null : mockCommentModelsList[i - 1].Id
                    );
                await commentRepo.Received(1).CreateAsync(mockCommentModelsList[i]);
                commentMapper.Received(1).ToCommentDto(mockCommentModelsList[i]);
            }
        }
    }
}
