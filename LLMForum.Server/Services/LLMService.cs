using LLMForum.Server.Exceptions;
using LLMForum.Server.Interfaces;
using Microsoft.SemanticKernel;

namespace LLMForum.Server.Services
{
    public class LLMService(Kernel kernel) : ILLMService
    {
        private readonly Kernel _kernel = kernel;
        private readonly int _commentCount = 3;

        public async Task<List<string>> GenerateCommentAsync(string promptText)
        {
            var changingPrompt = promptText;
            List<string> result = [changingPrompt];

            foreach (var num in Enumerable.Range(1, _commentCount))
            {
                var AIResult =
                    await _kernel.InvokePromptAsync<string>(changingPrompt)
                    ?? throw new AICommentGenerationFailedException(
                        "Failed to generate AI comment!"
                    );

                result.Add(AIResult);
                changingPrompt += $"/n AI Comment: {AIResult}";
            }
            result.RemoveAt(0);
            return result;
        }
    }
}
