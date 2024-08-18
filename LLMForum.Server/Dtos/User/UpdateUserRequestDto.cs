namespace LLMForum.Server.Dtos.User
{
    public class UpdateUserRequestDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
