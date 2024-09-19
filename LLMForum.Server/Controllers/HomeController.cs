using System.Security.Claims;
using LLMForum.Server.Dtos.AppUser;
using LLMForum.Server.Exceptions;
using LLMForum.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMForum.Server.Controllers
{
    [Route("api/home")]
    public class HomeController(IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserPosts()
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new NotFoundException("User Id");

            var posts = await _postService.GetUserPostsAsync(userId);

            return Ok(posts);
        }

        [Authorize]
        [HttpPost("prompt")]
        public async Task<IActionResult> CreateLLMResponse([FromBody] AppUserPromptDto promptDto)
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new NotFoundException("User Id");

            var postPage = await _postService.GetInitialPostPageAsync(userId, promptDto.Prompt);

            return Ok(postPage);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            await _postService.DeletePostAsync(id);
            return Ok(new { message = "Post deleted successfully" });
        }
    }
}
