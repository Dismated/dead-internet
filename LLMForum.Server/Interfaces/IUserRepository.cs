using LLMForum.Server.Dtos.User;
using LLMForum.Server.Models;

namespace LLMForum.Server.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(User userModel);
        Task<User?> UpdateAsync(int id, UpdateUserRequestDto userDto);
        Task<User?> DeleteAsync(int id);
    }
}
