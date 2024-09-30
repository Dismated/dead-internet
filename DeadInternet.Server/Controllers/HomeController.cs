using System.Security.Claims;
using DeadInternet.Server.Dtos.AppUser;
using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Dtos.Common;
using DeadInternet.Server.Dtos.Post;
using DeadInternet.Server.Exceptions.Base;
using DeadInternet.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeadInternet.Server.Controllers
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

            return Ok(new ApiResponse<List<PostDto>>(posts));
        }

        [Authorize]
        [HttpPost("prompt")]
        public async Task<IActionResult> CreateLLMResponse([FromBody] AppUserPromptDto promptDto)
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new NotFoundException("User Id");

            var postPage = await _postService.GetInitialPostPageAsync(userId, promptDto.Prompt);

            return Ok(new ApiResponse<PromptNRepliesDto>(postPage));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            await _postService.DeletePostAsync(id);
            return Ok(new MessageResponse("Post deleted successfully!"));
        }
    }
}
