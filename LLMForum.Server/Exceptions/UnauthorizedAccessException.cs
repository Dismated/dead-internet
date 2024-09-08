namespace LLMForum.Server.Exceptions
{
    public class UnauthorizedAccessException(string message) : ApplicationException(message) { }
}
