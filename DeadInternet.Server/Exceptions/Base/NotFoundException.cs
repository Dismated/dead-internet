namespace DeadInternet.Server.Exceptions.Base
{
    public class NotFoundException(string entityName)
        : ApplicationException($"{entityName} not found") { }
}
