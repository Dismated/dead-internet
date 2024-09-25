using LLMForum.Server.Dtos.Comment;

namespace LLMForum.Tests.MockData
{
    public static class LLMMockData
    {
        public static CreateCommentRequestDto GetMockCreateCommentRequestDto()
        {
            return new CreateCommentRequestDto
            {
                UserPrompt = "This is a mock user prompt.",
                PostId = Guid.NewGuid().ToString(),
            };
        }
    }
}
