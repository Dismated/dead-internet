using DeadInternet.Server.Dtos.Account;
using DeadInternet.Server.Exceptions.Base;
using DeadInternet.Server.Exceptions.User;
using DeadInternet.Server.Interfaces;
using DeadInternet.Server.Models;
using DeadInternet.Server.Services;
using DeadInternet.Tests.MockData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace DeadInternet.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly IAccountRepository _userRepo;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAccountService _accountService;
        private readonly LoginDto _mockLoginDto;
        private readonly AppUser _mockUser;
        private readonly NewAppUserDto _mockNewAppUserDto;

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

            _mockUser = UserMockData.GetMockUser();
            _mockLoginDto = UserMockData.GetMockLoginDto();
            _mockNewAppUserDto = UserMockData.GetMockNewAppUserDto();

            _accountService = new AccountService(
                _userRepo,
                _signInManager,
                _tokenService,
                _userManager
            );
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
                () => _accountService.GetUserByUsernameAsync(_mockLoginDto.Username)
            );
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ReturnsNewAppUserDto_WhenUserExists()
        {
            //Arrange
            _userRepo
                .GetUserByUsernameAsync(_mockLoginDto.Username)
                .Returns(Task.FromResult<AppUser?>(_mockUser));
            _signInManager
                .CheckPasswordSignInAsync(_mockUser, _mockLoginDto.Password, false)
                .Returns(Task.FromResult(SignInResult.Success));

            //Act
            var result = await _accountService.GetUserByUsernameAsync(_mockLoginDto.Username);

            //Assert
            Assert.IsType<AppUser>(result);
            Assert.Equal(_mockLoginDto.Username, result.UserName);
        }

        [Fact]
        public void CreateToken_ReturnsValidJwtToken()
        {
            //Arrange
            _tokenService.CreateToken(Arg.Any<AppUser>()).Returns(_mockNewAppUserDto.Token);

            //Act
            var result = _accountService.CreateToken(_mockUser);

            //Assert
            Assert.Equal(_mockNewAppUserDto.Token, result);
        }

        [Fact]
        public async Task GetVerifiedUserAsync_IncorrectPassword_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            _accountService
                .GetUserByUsernameAsync(Arg.Any<string>())
                .Returns(Task.FromResult(_mockUser));
            _signInManager
                .CheckPasswordSignInAsync(Arg.Any<AppUser>(), Arg.Any<string>(), false)
                .Returns(Task.FromResult(SignInResult.Failed));

            //Act & Assert
            await Assert.ThrowsAsync<CustomUnauthorizedAccessException>(
                () => _accountService.GetVerifiedUserAsync(_mockLoginDto)
            );
        }

        [Fact]
        public async Task GetVerifiedUserAsync_EmailNotFound_ThrowsNotFoundException()
        {
            //Arrange
            _mockUser.Email = null;
            _accountService
                .GetUserByUsernameAsync(_mockLoginDto.Username)
                .Returns(Task.FromResult(_mockUser));
            _signInManager
                .CheckPasswordSignInAsync(_mockUser, _mockLoginDto.Password, false)
                .Returns(Task.FromResult(SignInResult.Success));

            //Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _accountService.GetVerifiedUserAsync(_mockLoginDto)
            );
        }

        [Fact]
        public async Task GetVerifiedUserAsync_ReturnsNewAppUserDto_WhenUserExists()
        {
            //Arrange
            _accountService
                .GetUserByUsernameAsync(_mockLoginDto.Username)
                .Returns(Task.FromResult(_mockUser));
            _signInManager
                .CheckPasswordSignInAsync(_mockUser, _mockLoginDto.Password, false)
                .Returns(Task.FromResult(SignInResult.Success));
            _accountService.CreateToken(_mockUser).Returns(_mockNewAppUserDto.Token);

            //Act
            var result = await _accountService.GetVerifiedUserAsync(_mockLoginDto);

            //Assert
            Assert.IsType<NewAppUserDto>(result);
            Assert.Equal(_mockNewAppUserDto.Token, result.Token);
        }

        [Fact]
        public void GeneratePassword_ReturnsValidPassword()
        {
            //Act
            var result = _accountService.GeneratePassword();

            //Assert
            Assert.Equal(12, result.Length);
            Assert.Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()])(?=.*[a-z]).{12,}$", result);
        }

        [Theory]
        [InlineData(8)]
        [InlineData(12)]
        [InlineData(16)]
        [InlineData(20)]
        public void GeneratePassword_ReturnsPasswordOfSpecifiedLength_WithExplicitLength(int length)
        {
            // Act
            string result = _accountService.GeneratePassword(length);

            // Assert
            Assert.Equal(length, result.Length);
            Assert.Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()])(?=.*[a-z]).{8,}$", result);
        }

        [Fact]
        public void CreateGuestAccountAsync_ReturnsGuestUserDto()
        {
            //Act
            var result = _accountService.CreateGuestAccount();

            //Assert
            Assert.IsType<RegisterDto>(result);
        }

        [Fact]
        public async Task CreateAccountAsync_FailedToCreateUser_ThrowsUserCreationException()
        {
            //Arrange
            var mockRegisterDto = UserMockData.GetMockRegisterDto();

            _userManager
                .CreateAsync(Arg.Any<AppUser>(), Arg.Any<string>())
                .Returns(Task.FromResult(IdentityResult.Failed()));

            //Act & Assert
            await Assert.ThrowsAsync<UserCreationException>(
                () => _accountService.CreateAccountAsync(mockRegisterDto)
            );
        }

        [Fact]
        public async Task CreateAccountAsync_CreatesValidUser()
        {
            //Arrange
            var mockRegisterDto = UserMockData.GetMockRegisterDto();

            _userManager
                .CreateAsync(Arg.Any<AppUser>(), mockRegisterDto.Password)
                .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            await _accountService.CreateAccountAsync(mockRegisterDto);

            //Assert
            await _userManager
                .Received(1)
                .CreateAsync(
                    Arg.Is<AppUser>(u => u.UserName == mockRegisterDto.Username),
                    mockRegisterDto.Password
                );
        }
    }
}
