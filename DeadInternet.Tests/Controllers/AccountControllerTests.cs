using DeadInternet.Tests.MockData;
using NSubstitute;

namespace DeadInternet.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly UserManager<AppUser> _userManager;
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
            _accountService = Substitute.For<IAccountService>();
            _controller = new AccountController(_accountService, _userManager);
        }

        [Fact]
        public async Task Login_ReturnsOk_WithNewAppUserDto()
        {
            //Arrange
            var mockUser = UserMockData.GetMockNewAppUserDto();
            var mockLoginDto = UserMockData.GetMockLoginDto();
            _accountService.GetUserByUsernameAsync(mockLoginDto).Returns(Task.FromResult(mockUser));

            //Act
            var result = await _controller.Login(mockLoginDto);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<NewAppUserDto>(okResult.Value);
            Assert.Equal(mockUser.Username, dto.Username);
            Assert.Equal(mockUser.Email, dto.Email);
            Assert.Equal(mockUser.Token, dto.Token);
        }

        [Fact]
        public async Task Register_ReturnsStatusCode500_WhenUserAlreadyExists()
        {
            //Arrange
            var description = "Username already exists!";
            var mockRegisterDto = UserMockData.GetMockRegisterDto();
            var error = new IdentityError { Description = description };

            _userManager
                .CreateAsync(Arg.Any<AppUser>(), Arg.Any<string>())
                .Returns(Task.FromResult(IdentityResult.Failed(error)));

            //Act
            var result = await _controller.Register(mockRegisterDto);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objResult.StatusCode);
            Assert.NotNull(objResult.Value);
            Assert.Equal(description, ((List<IdentityError>)objResult.Value)[0].Description);
        }

        [Fact]
        public async Task Register_ReturnsStatusCode500_WhenFailedToAssignRole()
        {
            //Arrange
            var mockUser = UserMockData.GetMockUser();
            var mockregisterDto = UserMockData.GetMockRegisterDto();
            var description = "Failed to assign role!";
            var error = new IdentityError { Description = description };

            _userManager
                .CreateAsync(Arg.Any<AppUser>(), Arg.Any<string>())
                .Returns(Task.FromResult(IdentityResult.Success));
            _userManager
                .AddToRoleAsync(Arg.Any<AppUser>(), Arg.Any<string>())
                .Returns(Task.FromResult(IdentityResult.Failed(error)));

            //Act
            var result = await _controller.Register(mockregisterDto);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objResult.StatusCode);
            Assert.NotNull(objResult.Value);
            Assert.Equal(description, ((List<IdentityError>)objResult.Value)[0].Description);
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
