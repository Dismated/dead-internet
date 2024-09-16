using System.Security.Claims;
using LLMForum.Server.Dtos.AppUser;
using LLMForum.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMForum.Server.Controllers
{
    [Route("api/home")]
    public class HomeController(
        IPostService postService,
        ICommentService commentService,
        IPostRepository postRepository
    ) : ControllerBase
    {
        private readonly IPostService _postService = postService;
        private readonly ICommentService _commentService = commentService;
        private readonly IPostRepository _postRepo = postRepository;

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
            var postId = await _postService.GetPostIdAsync(userId, promptDto.Prompt);
            var promptModel = await _commentService.CreatePromptAsync(promptDto.Prompt, postId);
            await _commentService.CreateCommentsAsync(promptDto.Prompt, postId, promptModel.Id);

            var comments = await _commentService.ReturnThreadAsync(promptModel.Id);
            Console.WriteLine(comments.ToString());

            return Ok(comments);
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
