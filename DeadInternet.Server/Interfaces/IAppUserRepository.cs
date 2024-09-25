using DeadInternet.Server.Dtos.AppUser;
using DeadInternet.Server.Models;

namespace DeadInternet.Server.Interfaces
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
