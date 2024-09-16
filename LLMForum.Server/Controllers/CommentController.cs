using LLMForum.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/comments")]
public class CommentController(ICommentService commentService, ICommentMapper commentMapper)
    : ControllerBase
{
    private readonly ICommentService _commentService = commentService;
    private readonly ICommentMapper _commentMapper = commentMapper;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var comment = await _commentService.GetCommentAsync(id);
        return Ok(_commentMapper.ToCommentDto(comment));
    }

    [HttpGet("post/{postId}")]
    public async Task<IActionResult> GetByPostId([FromRoute] string postId)
    {
        var comments = await _commentService.ReturnThreadAsync(postId);

        return Ok(comments);
    }
}
