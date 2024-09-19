namespace LLMForum.Server.Exceptions
{
    public class BadRequestException(string message) : Base.ApplicationException(message) { }
}
