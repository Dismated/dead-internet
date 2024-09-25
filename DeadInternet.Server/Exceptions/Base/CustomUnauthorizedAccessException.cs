namespace DeadInternet.Server.Exceptions.Base
{
    public class CustomUnauthorizedAccessException(string message)
        : ApplicationException(message) { }
}
