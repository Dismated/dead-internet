using DeadInternet.Server.Controllers;
using DeadInternet.Server.Dtos.Account;
using DeadInternet.Server.Dtos.Common;
using DeadInternet.Server.Interfaces;
using DeadInternet.Tests.MockData;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace DeadInternet.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly ITokenService _tokenService;
        private readonly AccountController _controller;
        private readonly IAccountService _accountService;

        public AccountControllerTests()
        {
            _accountService = Substitute.For<IAccountService>();
            _tokenService = Substitute.For<ITokenService>();
            _controller = new AccountController(_accountService);
        }

        [Fact]
        public async Task Login_ReturnsOk_WithNewAppUserDto()
        {
            //Arrange
            var mockUser = UserMockData.GetMockNewAppUserDto();
            var mockLoginDto = UserMockData.GetMockLoginDto();
            _accountService.GetVerifiedUserAsync(mockLoginDto).Returns(Task.FromResult(mockUser));

            //Act
            var result = await _controller.Login(mockLoginDto);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ApiResponse<NewAppUserDto>>(okResult.Value);
            Assert.Equal(mockUser, dto.Data);
        }

        [Fact]
        public async Task Register_ReturnsOk_WithMessage()
        {
            //Arrange
            var message = "Account created successfully!";
            var mockRegisterDto = UserMockData.GetMockRegisterDto();

            _accountService.CreateAccountAsync(mockRegisterDto).Returns(Task.CompletedTask);

            //Act
            var result = await _controller.Register(mockRegisterDto);

            //Assert
            var objResult = Assert.IsType<OkObjectResult>(result);
            var messageResult = Assert.IsType<MessageResponse>(objResult.Value);
            Assert.Equal(message, messageResult.Message);
        }

        [Fact]
        public async Task LoginAsGuest_ReturnsOk_WithToken()
        {
            //Arrange
            var mockRegisterDto = UserMockData.GetMockRegisterDto();
            var mockUser = UserMockData.GetMockUser(mockRegisterDto);
            var token = "FakeJwtToken";
            _accountService.CreateGuestAccount().Returns(mockRegisterDto);
            _accountService.CreateAccountAsync(mockRegisterDto).Returns(Task.CompletedTask);
            _accountService
                .GetUserByUsernameAsync(mockRegisterDto.Username)
                .Returns(Task.FromResult(mockUser));
            _accountService.CreateToken(mockUser).Returns(token);

            //Act
            var result = await _controller.LoginAsGuest();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenResult = Assert.IsType<ApiResponse<TokenResponse>>(okResult.Value);
            Assert.Equal(token, tokenResult.Data.Token);
        }
    }
}
