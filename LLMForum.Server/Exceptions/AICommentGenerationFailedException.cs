namespace LLMForum.Server.Exceptions
{
    public class AICommentGenerationFailedException(string message)
        : ApplicationException(message) { }
}
