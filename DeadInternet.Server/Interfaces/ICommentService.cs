using DeadInternet.Server.Dtos.Comment;
using DeadInternet.Server.Models;

namespace DeadInternet.Server.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> GetCommentAsync(string Id);
        Task<Comment> CreatePromptAsync(string promptText, string postId);
        Task CreateCommentsAsync(CreateCommentDto createCommentDto);
        Task<List<CommentDto>> GetPostCommentsAsync(string postId);
        Task<CommentDto> GetPromptDtoAsync(string postId);
        Task<List<CommentDto>> GetRepliesDtoAsync(string commentId);
        Task<PromptNRepliesDto> GetPromptNRepliesAsync(CommentDto promptDto);
        Task DeleteCommentChainAsync(string commentId);
        Task UpdateCommentAsync(string commentId, string content);
        Task<string> CreateReplyAsync(ReplyDto replyDto, string postId);
    }
}
