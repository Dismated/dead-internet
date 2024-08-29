using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Mappers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CommentController(ICommentRepository commentRepo, ICommentService commentService)
    : ControllerBase
{
    private readonly ICommentRepository _commentRepo = commentRepo;
    private readonly ICommentService _commentService = commentService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var comment = await _commentRepo.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment.ToCommentDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommentRequestDto commentDto)
    {
        var savedComments = await _commentService.CreateComments(commentDto);

        var response = new
        {
            Comments = savedComments,
            Message = $"Successfully created {savedComments.Count} comments",
        };

        return CreatedAtAction(nameof(GetById), new { id = savedComments[0].Id }, response);
    }
}
