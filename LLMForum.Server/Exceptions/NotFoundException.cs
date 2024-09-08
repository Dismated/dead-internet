namespace LLMForum.Server.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string entityName)
            : base($"{entityName} not found") { }
    }
}
