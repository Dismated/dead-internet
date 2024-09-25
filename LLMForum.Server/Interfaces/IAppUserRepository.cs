using LLMForum.Server.Dtos.AppUser;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface IAppUserRepository
    {
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser?> GetByIdAsync(string id);
        Task<AppUser> CreateAsync(AppUser userModel);
        Task<AppUser?> UpdateAsync(string id, UpdateAppUserRequestDto userDto);
        Task<AppUser?> DeleteAsync(string id);
    }
}
