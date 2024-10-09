using DeadInternet.Server.Dtos.Account;
using DeadInternet.Server.Dtos.Common;
using DeadInternet.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeadInternet.Server.Controllers
{
    [Route("api/account")]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _accountService.GetVerifiedUserAsync(loginDto);
            return Ok(new ApiResponse<NewAppUserDto>(user));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            await _accountService.CreateAccountAsync(registerDto);
            return Ok(new MessageResponse("Account created successfully!"));
        }

        [HttpPost("guest/login")]
        public async Task<IActionResult> LoginAsGuest()
        {
            var guest = _accountService.CreateGuestAccount();
            await _accountService.CreateAccountAsync(guest);

            var user = await _accountService.GetUserByUsernameAsync(guest.Username);
            var token = _accountService.CreateToken(user);
            return Ok(new ApiResponse<TokenResponse>(new TokenResponse(token)));
        }
    }
}
