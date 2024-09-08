using LLMForum.Server.Controllers;
using LLMForum.Server.Dtos.Account;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Models;
using LLMForum.Tests.MockData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace LLMForum.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly AccountController _controller;
        private readonly IAccountService _accountService;

        public AccountControllerTests()
        {
            _userManager = Substitute.For<UserManager<AppUser>>(
                Substitute.For<IUserStore<AppUser>>(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );
            _signInManager = Substitute.For<SignInManager<AppUser>>(
                _userManager,
                Substitute.For<IHttpContextAccessor>(),
                Substitute.For<IUserClaimsPrincipalFactory<AppUser>>(),
                null,
                null,
                null,
                null
            );
            _accountService = Substitute.For<IAccountService>();
            _controller = new AccountController(_userManager, _signInManager, _accountService);
        }

        [Fact]
        public async Task Login_ReturnsOk_WithNewAppUserDto()
        {
            //Arrange
            var mockUser = UserMockData.GetMockUser();
            var mockLoginDto = UserMockData.GetMockLoginDto();
            var token = "FakeJWTToken";
            _accountService.GetUserByUsernameAsync(mockLoginDto).Returns(Task.FromResult(mockUser));

            //Act
            var result = await _controller.Login(mockLoginDto);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<NewAppUserDto>(okResult.Value);
            Assert.Equal(mockUser.UserName, dto.UserName);
            Assert.Equal(mockUser.Email, dto.Email);
            Assert.Equal(token, dto.Token);
        }

        public async Task Register_ReturnsStatusCode500_WhenUserAlreadyExists()
        {
            //Arrange
            var mockRegisterDto = UserMockData.GetMockRegisterDto();
            _userManager
                .CreateAsync(Arg.Any<AppUser>(), Arg.Any<string>())
                .Returns(
                    Task.FromResult(
                        IdentityResult.Failed(
                            new IdentityError { Description = "Username already exists!" }
                        )
                    )
                );

            //Act
            var result = await _controller.Register(mockRegisterDto);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objResult.StatusCode);
            Assert.Contains("Username already exists!", objResult.Value.ToString());
        }

        public async Task Register_ReturnsStatusCode500_WhenFailedToAssignRole()
        {
            //Arrange
            var mockUser = UserMockData.GetMockUser();
            var mockregisterDto = UserMockData.GetMockRegisterDto();
            _userManager
                .CreateAsync(Arg.Any<AppUser>(), Arg.Any<string>())
                .Returns(Task.FromResult(IdentityResult.Success));
            _userManager
                .AddToRoleAsync(Arg.Any<AppUser>(), Arg.Any<string>())
                .Returns(
                    Task.FromResult(
                        IdentityResult.Failed(
                            new IdentityError { Description = "Failed to assign role!" }
                        )
                    )
                );

            //Act
            var result = await _controller.Register(mockregisterDto);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objResult.StatusCode);
            Assert.Contains("Failed to assign role!", objResult.Value.ToString());
        }

        [Fact]
        public async Task Register_ReturnsStatusCode500_WhenExceptionIsThrown()
        {
            // Arrange
            _userManager
                .CreateAsync(Arg.Any<AppUser>(), Arg.Any<string>())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Register(UserMockData.GetMockRegisterDto());

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Test exception", ((Exception)statusCodeResult.Value).Message);
        }

        [Fact]
        public async Task Register_ReturnsOk_WithSuccessMessage()
        {
            //Arrange
            var mockregisterDto = UserMockData.GetMockRegisterDto();
            var appUser = new AppUser
            {
                Email = mockregisterDto.Email,
                UserName = mockregisterDto.Username,
            };
            _userManager
                .CreateAsync(Arg.Any<AppUser>(), Arg.Any<string>())
                .Returns(Task.FromResult(IdentityResult.Success));
            _userManager
                .AddToRoleAsync(Arg.Any<AppUser>(), "User")
                .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = await _controller.Register(mockregisterDto);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User created", okResult.Value);
        }
    }
}
