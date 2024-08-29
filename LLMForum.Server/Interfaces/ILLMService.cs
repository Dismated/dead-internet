using LLMForum.Server.Dtos.Comment;

namespace LLMForum.Server.Interfaces
{
    public interface ILLMService
    {
        Task<List<string>> GenerateCommentAsync(CreateCommentRequestDto commentDto);
    }
}
