namespace DeadInternet.Server.Exceptions.User
{
    public class UserCreationException(string message)
        : BadRequestException($"Failed to create user: {message}") { }
}
