using DeadInternet.Server.Dtos.Account;
using DeadInternet.Server.Models;

namespace DeadInternet.Tests.MockData
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

        public static AppUser GetMockUser(RegisterDto registerDto)
        {
            return new AppUser
            {
                UserName = registerDto.Username,
                PasswordHash = registerDto.Password,
                Email = registerDto.Email,
            };
        }

        public static NewAppUserDto GetMockNewAppUserDto()
        {
            return new NewAppUserDto
            {
                Username = "username",
                Email = "test@test.com",
                Token = "FakeJWTToken",
            };
        }

        public static LoginDto GetMockLoginDto()
        {
            return new LoginDto { Username = "username", Password = "password" };
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
