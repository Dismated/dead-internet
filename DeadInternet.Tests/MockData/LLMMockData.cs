namespace DeadInternet.Tests.MockData
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
