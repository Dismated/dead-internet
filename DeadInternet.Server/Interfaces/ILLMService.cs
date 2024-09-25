namespace DeadInternet.Server.Interfaces
{
    public interface ILLMService
    {
        Task<List<string>> GenerateCommentAsync(string promptText);
    }
}
