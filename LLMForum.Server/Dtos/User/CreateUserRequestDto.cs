namespace LLMForum.Server.Dtos.User
{
    public class CreateUserRequestDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

}
