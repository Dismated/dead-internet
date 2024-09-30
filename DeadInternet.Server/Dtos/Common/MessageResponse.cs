namespace DeadInternet.Server.Dtos.Common
{
    public class MessageResponse(string message)
    {
        public string Message { get; set; } = message;
    }
}
