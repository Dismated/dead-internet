namespace LLMForum.Server.Exceptions
{
    public class FailedToRegisterException(string message) : ApplicationException(message) { }
}
