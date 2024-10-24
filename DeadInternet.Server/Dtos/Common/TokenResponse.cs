namespace DeadInternet.Server.Dtos.Common
{
    public class TokenResponse(string token)
    {
        public string Token { get; set; } = token;
    }
}
