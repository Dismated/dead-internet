using DeadInternet.Server.Models;

namespace DeadInternet.Server.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
