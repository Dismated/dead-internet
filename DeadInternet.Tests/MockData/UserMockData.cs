﻿namespace DeadInternet.Tests.MockData
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
