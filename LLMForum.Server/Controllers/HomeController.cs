using System.Security.Claims;
using LLMForum.Server.Dtos.AppUser;
using LLMForum.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMForum.Server.Controllers
{
    [Route("api/home")]
    public class HomeController(IPostService postService, ICommentService commentService)
        : ControllerBase
    {
        private readonly IPostService _postService = postService;
        private readonly ICommentService _commentService = commentService;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserPosts()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var posts = await _postService.GetUserPostsAsync(userId);
            return Ok(posts);
        }

        [Authorize]
        [HttpPost("prompt")]
        public async Task<IActionResult> CreateLLMResponse([FromBody] AppUserPromptDto promptDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var postId = await _postService.GetPostIdAsync(userId);
            var comments = await _commentService.CreateComments(promptDto.Prompt, postId);

            return Ok(comments);
        }
    }
}
