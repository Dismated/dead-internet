using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using LLMForum.Server.Mappers;


[ApiController]
[Route("api/[controller]")]
public class CommentController(Kernel kernel, IPostRepository postRepo, ICommentRepository commentRepo, ILLMService ILLMService, ICommentService commentService) : ControllerBase
{
    private readonly Kernel _kernel = kernel;
    private readonly IPostRepository _postRepo = postRepo;
    private readonly ICommentRepository _commentRepo = commentRepo;
    private readonly ILLMService _ILLMService = ILLMService;
    private readonly ICommentService _commentService = commentService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
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
            Message = $"Successfully created {savedComments.Count} comments"
        };


        return CreatedAtAction(nameof(GetById), new { id = savedComments[0].Id }, response);
    }


}
