using LLMForum.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/comments")]
public class CommentController(
    ICommentService commentService,
    ICommentMapper commentMapper,
    IPostService postService
) : ControllerBase
{
    private readonly ICommentService _commentService = commentService;
    private readonly ICommentMapper _commentMapper = commentMapper;
    private readonly IPostService _postService = postService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var comment = await _commentService.GetCommentAsync(id);
        return Ok(_commentMapper.ToCommentDto(comment));
    }

    [HttpGet("post/{postId}")]
    public async Task<IActionResult> GetPromptNRepliesByPostId([FromRoute] string postId)
    {
        var promptNReplies = await _postService.GetPostPageAsync(postId);

        return Ok(promptNReplies);
    }

    [HttpDelete("{commentId}")]
    public async Task<IActionResult> DeleteCommentChain([FromRoute] string commentId)
    {
        await _commentService.DeleteCommentChainAsync(commentId);
        return Ok(new { message = "Post deleted successfully" });
    }
}
