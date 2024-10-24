using DeadInternet.Server.Services;
using DeadInternet.Tests.MockData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using NSubstitute;

namespace DeadInternet.Tests.Services
{
    public class LLMServiceTests
    {
        [Fact]
        public async Task GenerateCommentAsync_ReturnsCommentsList()
        {
            //Arrange
            var mockCommentList = CommentMockData.GetMockCommentsList();

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
            var result = await LLMService.GenerateCommentAsync("This is a prompt!");

            //Assert
            Assert.Equal(mockCommentList, result);
        }
    }
}
