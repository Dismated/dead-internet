using LLMForum.Server.Services;
using LLMForum.Tests.MockData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using NSubstitute;

namespace LLMForum.Tests.Services
{
    public class LLMServiceTests
    {
        [Fact]
        public async Task GenerateCommentAsync_ReturnsCommentsList()
        {
            //Arrange
            var mockCommentList = CommentMockData.GetMockCommentsList();
            var mockCommentModel = LLMMockData.GetMockCreateCommentRequestDto();

            var chatCompletion = Substitute.For<IChatCompletionService>();
            chatCompletion
                .GetChatMessageContentsAsync(
                    Arg.Any<ChatHistory>(),
                    Arg.Any<PromptExecutionSettings>(),
                    Arg.Any<Kernel>(),
                    Arg.Any<CancellationToken>()
                )
                .Returns(
                    [new ChatMessageContent(AuthorRole.Assistant, mockCommentList[0])],
                    [new ChatMessageContent(AuthorRole.Assistant, mockCommentList[1])],
                    [new ChatMessageContent(AuthorRole.Assistant, mockCommentList[2])]
                );

            var kernelBuilder = Kernel.CreateBuilder();
            kernelBuilder.Services.AddSingleton(chatCompletion);

            var kernel = kernelBuilder.Build();
            var LLMService = new LLMService(kernel);

            //Act
            var result = await LLMService.GenerateCommentAsync(mockCommentModel);

            //Assert
            Assert.Equal(mockCommentList, result);
        }
    }
}
