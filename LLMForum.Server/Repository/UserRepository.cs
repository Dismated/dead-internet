using LLMForum.Server.Data;
using LLMForum.Server.Dtos.User;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LLMForum.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllAsync()
        {

            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);

        }

        public async Task<User?> UpdateAsync(int id, UpdateUserRequestDto userDto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (existingUser == null)
            {
                return null;
            }
            existingUser.Username = userDto.Username;
            existingUser.Email = userDto.Email;
            existingUser.PasswordHash = userDto.PasswordHash;

            await _context.SaveChangesAsync();
            return existingUser;

        }


        public async Task<User> CreateAsync(User userModel)
        {
            await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<User?> DeleteAsync(int id)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userModel == null)
            {
                return null;
            }
            _context.Users.Remove(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }
    }

}
