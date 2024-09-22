using DeadInternet.Server.Dtos.Account;
using DeadInternet.Server.Interfaces;
using DeadInternet.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeadInternet.Server.Controllers
{
    [Route("api/account")]
    public class AccountController(IAccountService accountService, UserManager<AppUser> userManager)
        : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _accountService.GetVerifiedUserAsync(loginDto);
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            await _accountService.CreateAccountAsync(registerDto);
            return Ok(new { message = "Account created successfully" });
        }

        [HttpPost("guest/login")]
        public async Task<IActionResult> LoginAsGuest()
        {
            var guest = _accountService.CreateGuestAccountAsync();
            await _accountService.CreateAccountAsync(guest);

            var user = await _accountService.GetUserByUsernameAsync(guest.Username);
            var token = _accountService.CreateToken(user);
            return Ok(new { token });
        }
    }
}
