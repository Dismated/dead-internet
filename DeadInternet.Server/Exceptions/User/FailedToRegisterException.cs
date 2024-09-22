namespace DeadInternet.Server.Exceptions.User
{
    public class FailedToRegisterException(string message) : Base.ApplicationException(message) { }
}
