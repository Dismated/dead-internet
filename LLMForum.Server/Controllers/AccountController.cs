using LLMForum.Server.Dtos.Account;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LLMForum.Server.Controllers
{
    [Route("api/account")]
    public class AccountController(IAccountService accountService, UserManager<AppUser> userManager)
        : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;
        private readonly UserManager<AppUser> _userManager = userManager;

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _accountService.GetUserByUsernameAsync(loginDto);

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };

            var createResult = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (createResult.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                {
                    return Ok("User created");
                }
                else
                {
                    return StatusCode(500, roleResult.Errors);
                }
            }
            else
            {
                return StatusCode(500, createResult.Errors);
            }
        }
    }
}
