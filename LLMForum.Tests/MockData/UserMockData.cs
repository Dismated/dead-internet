using LLMForum.Server.Dtos.Account;
using LLMForum.Server.Models;

namespace LLMForum.Tests.MockData
{
    internal class UserMockData
    {
        public static AppUser GetMockUser()
        {
            return new AppUser
            {
                UserName = "username",

                PasswordHash = "password",
                Email = "test@test.com",
            };
        }

        public static LoginDto GetMockLoginDto()
        {
            return new LoginDto { Username = "username", Password = "password" };
        }

        public static LoginDto GetMockLoginWrongUsernameDto()
        {
            return new LoginDto { Username = "wrongusername", Password = "password" };
        }

        public static LoginDto GetMockLoginWrongPasswordDto()
        {
            return new LoginDto { Username = "username", Password = "wrongpassword" };
        }

        public static RegisterDto GetMockRegisterDto()
        {
            return new RegisterDto
            {
                Username = "newUsername",
                Email = "test@test.com",
                Password = "password",
            };
        }
    }
}
