using DeadInternet.Tests.MockData;
using NSubstitute;

namespace DeadInternet.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly IAccountRepository _userRepo;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AccountService _accountService;
        private readonly LoginDto _mockLoginDto;
        private readonly AppUser mockUser;

        public AccountServiceTests()
        {
            _userRepo = Substitute.For<IAccountRepository>();
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
            _signInManager = _signInManager = Substitute.For<SignInManager<AppUser>>(
                _userManager,
                Substitute.For<IHttpContextAccessor>(),
                Substitute.For<IUserClaimsPrincipalFactory<AppUser>>(),
                null,
                null,
                null,
                null
            );
            _tokenService = Substitute.For<ITokenService>();

            mockUser = UserMockData.GetMockUser();
            _mockLoginDto = UserMockData.GetMockLoginDto();

            _accountService = new AccountService(_userRepo, _signInManager, _tokenService);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_UsernameNotFound_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            _userRepo
                .GetUserByUsernameAsync(Arg.Any<string>())
                .Returns(Task.FromResult<AppUser?>(null));

            //Act & Assert
            await Assert.ThrowsAsync<CustomUnauthorizedAccessException>(
                () => _accountService.GetUserByUsernameAsync(_mockLoginDto)
            );
        }

        [Fact]
        public async Task GetUserByUsernameAsync_PasswordIncorrect_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            _userRepo
                .GetUserByUsernameAsync(Arg.Any<string>())
                .Returns(Task.FromResult<AppUser?>(mockUser));
            _signInManager
                .CheckPasswordSignInAsync(Arg.Any<AppUser>(), Arg.Any<string>(), false)
                .Returns(Task.FromResult(SignInResult.Failed));

            //Act & Assert
            await Assert.ThrowsAsync<CustomUnauthorizedAccessException>(
                () => _accountService.GetUserByUsernameAsync(_mockLoginDto)
            );
        }

        [Fact]
        public async Task GetUserByUsernameAsync_EmailNotFound_ThrowsNotFoundException()
        {
            //Arrange
            mockUser.Email = null;

            _userRepo
                .GetUserByUsernameAsync(Arg.Any<string>())
                .Returns(Task.FromResult<AppUser?>(mockUser));
            _signInManager
                .CheckPasswordSignInAsync(mockUser, _mockLoginDto.Password, false)
                .Returns(Task.FromResult(SignInResult.Success));

            //Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _accountService.GetUserByUsernameAsync(_mockLoginDto)
            );
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ReturnsNewAppUserDto_WhenUserExists()
        {
            //Arrange
            _userRepo
                .GetUserByUsernameAsync(_mockLoginDto.Username)
                .Returns(Task.FromResult<AppUser?>(mockUser));
            _signInManager
                .CheckPasswordSignInAsync(mockUser, _mockLoginDto.Password, false)
                .Returns(Task.FromResult(SignInResult.Success));

            //Act
            var result = await _accountService.GetUserByUsernameAsync(_mockLoginDto);

            //Assert
            Assert.IsType<NewAppUserDto>(result);
            Assert.Equal(_mockLoginDto.Username, result.Username);
        }
    }
}
