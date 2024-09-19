using LLMForum.Server.Dtos.Comment;
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

    [HttpPut("{commentId}")]
    public async Task<IActionResult> UpdateComment(
        [FromRoute] string commentId,
        [FromBody] UpdateCommentContentDto comment
    )
    {
        await _commentService.UpdateCommentAsync(commentId, comment.Content);
        var postId = await _postService.GetPostIdFromCommentAsync(commentId);
        await _commentService.CreateCommentsAsync(comment.Content, postId, commentId);
        var replies = await _commentService.GetRepliesDtoAsync(commentId);

        return Ok(replies);
    }

    [HttpPost("reply")]
    public async Task<IActionResult> CreateReply([FromBody] ReplyDto replyDto)
    {
        var postId = await _postService.GetPostIdFromCommentAsync(replyDto.ParentCommentId);
        var replyId = await _commentService.CreateReplyAsync(replyDto, postId);
        await _commentService.CreateCommentsAsync(replyDto.Content, postId, replyId);

        return Ok(new { message = "Reply created successfully" });
    }
}
