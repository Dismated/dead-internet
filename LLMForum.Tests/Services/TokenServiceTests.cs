using System.IdentityModel.Tokens.Jwt;
using System.Text;
using LLMForum.Server.Services;
using LLMForum.Tests.MockData;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;

namespace Token.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly IConfiguration _config;
        private readonly TokenService _tokenService;
        private readonly SymmetricSecurityKey _key;

        public TokenServiceTests()
        {
            _config = Substitute.For<IConfiguration>();
            _config["JWT:SigningKey"]
                .Returns(
                    "ja;ljfashnjfhj13j4oij451h35h1i3b5k1jh3jklahlkfjhhjlkja1234y01294hi1o2huh1oi34uho1i2uh34oi12g"
                );
            _config["JWT:Issuer"].Returns("TestIssuer");
            _config["JWT:Audience"].Returns("TestAudience");

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));

            _tokenService = new TokenService(_config);
        }

        [Fact]
        public void CreateToken_Returns_ValidJwtToken()
        {
            // Arrange
            var mockUser = UserMockData.GetMockUser();
            var tokenHandler = new JwtSecurityTokenHandler();

            // Act
            var token = _tokenService.CreateToken(mockUser);

            // Assert
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _key,
                    ValidateIssuer = true,
                    ValidIssuer = "TestIssuer",
                    ValidateAudience = true,
                    ValidAudience = "TestAudience",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                },
                out SecurityToken validatedToken
            );

            var jwtToken = (JwtSecurityToken)validatedToken;

            Assert.Equal(
                mockUser.Email,
                jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value
            );
            Assert.Equal(
                mockUser.UserName,
                jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.GivenName).Value
            );
        }
    }
}
