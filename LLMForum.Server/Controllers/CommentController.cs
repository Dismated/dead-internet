using LLMForum.Server.Dtos.Comment;
using LLMForum.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using LLMForum.Server.Mappers;


[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly Kernel _kernel;
    private readonly IPostRepository _postRepo;
    private readonly ICommentRepository _commentRepo;
    public CommentController(Kernel kernel, IPostRepository postRepo, ICommentRepository commentRepo)
    {
        _kernel = kernel;
        _postRepo = postRepo;
        _commentRepo = commentRepo;
    }

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
        var aiComments = await _commentRepo.GenerateCommentAsync(commentDto);

        var savedComments = new List<CommentDto>();
        foreach (var comment in aiComments)
        {
            var parentCommentId = savedComments.LastOrDefault()?.Id;
            var commentModel = commentDto.ToCommentFromCreateDTO(comment, parentCommentId);
            await _commentRepo.CreateAsync(commentModel);
            savedComments.Add(commentModel.ToCommentDto());
        }

        var response = new
        {
            Comments = savedComments,
            Message = $"Successfully created {savedComments.Count} comments"
        };


        return CreatedAtAction(nameof(GetById), new { id = savedComments[0].Id }, response);
    }


}
