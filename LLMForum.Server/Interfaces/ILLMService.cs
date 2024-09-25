namespace LLMForum.Server.Interfaces
{
    public interface ILLMService
    {
        Task<List<string>> GenerateCommentAsync(string promptText);
    }
}
