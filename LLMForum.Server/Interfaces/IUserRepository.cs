using LLMForum.Server.Dtos.User;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser?> GetByIdAsync(int id);
        Task<AppUser> CreateAsync(AppUser userModel);
        Task<AppUser?> UpdateAsync(int id, UpdateUserRequestDto userDto);
        Task<AppUser?> DeleteAsync(int id);
    }
}
