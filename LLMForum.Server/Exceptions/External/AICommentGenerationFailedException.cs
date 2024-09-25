namespace LLMForum.Server.Exceptions.External
{
    public class AICommentGenerationFailedException(string message)
        : Base.ApplicationException(message) { }
}
