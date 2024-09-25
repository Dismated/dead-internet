using DeadInternet.Server.Models;

namespace DeadInternet.Server.Interfaces
{
    public interface IAccountRepository
    {
        Task<AppUser?> GetUserByUsernameAsync(string username);
    }
}
