using LLMForum.Server.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace LLMForum.Server.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(string id);
        Task CreateAsync(Comment commentDto);

        Task SaveChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
