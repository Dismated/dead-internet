namespace LLMForum.Server.Exceptions
{
    public class CustomUnauthorizedAccessException(string message)
        : ApplicationException(message) { }
}
