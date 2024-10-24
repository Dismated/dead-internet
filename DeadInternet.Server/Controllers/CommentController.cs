using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Dtos.Common;
using DeadInternet.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var comment = await _commentService.GetCommentAsync(id);
        return Ok(new ApiResponse<CommentDto>(_commentMapper.ToCommentDto(comment)));
    }

    [Authorize]
    [HttpGet("post/{postId}")]
    public async Task<IActionResult> GetPromptNRepliesByPostId([FromRoute] string postId)
    {
        var promptNReplies = await _postService.GetPostPageAsync(postId);
        return Ok(new ApiResponse<PromptNRepliesDto>(promptNReplies));
    }

    [Authorize]
    [HttpDelete("{commentId}")]
    public async Task<IActionResult> DeleteCommentChain([FromRoute] string commentId)
    {
        await _commentService.DeleteCommentChainAsync(commentId);
        return Ok(new MessageResponse("Post deleted successfully!"));
    }

    [Authorize]
    [HttpPut("{commentId}")]
    public async Task<IActionResult> UpdateComment(
        [FromRoute] string commentId,
        [FromBody] UpdateCommentContentDto comment
    )
    {
        await _commentService.UpdateCommentAsync(commentId, comment.Content);
        var postId = await _postService.GetPostIdFromCommentAsync(commentId);

        var createCommentDto = new CreateCommentDto
        {
            PromptText = comment.Content,
            PostId = postId,
            ParentCommentId = commentId,
        };

        await _commentService.CreateCommentsAsync(createCommentDto);
        var replies = await _commentService.GetRepliesDtoAsync(commentId);

        return Ok(new ApiResponse<List<CommentDto>>(replies));
    }

    [Authorize]
    [HttpPost("reply")]
    public async Task<IActionResult> CreateReply([FromBody] ReplyDto replyDto)
    {
        var postId = await _postService.GetPostIdFromCommentAsync(replyDto.ParentCommentId);
        var replyId = await _commentService.CreateReplyAsync(replyDto, postId);

        var createCommentDto = new CreateCommentDto
        {
            PromptText = replyDto.Content,
            PostId = postId,
            ParentCommentId = replyId,
        };
        await _commentService.CreateCommentsAsync(createCommentDto);

        return Ok(new MessageResponse("Reply created successfully!"));
    }
}
