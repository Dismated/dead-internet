using DeadInternet.Server.Interfaces;
using DeadInternet.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DeadInternet.Server.Repositories
{
    public class AccountRepository(UserManager<AppUser> userManager) : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }
    }
}
