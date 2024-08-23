using LLMForum.Server.Dtos.User;
using LLMForum.Server.Models;

namespace LLMForum.Server.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToUserDto(this AppUser userModel)
        {
            return new UserDto
            {
                Id = userModel.Id,
                Email = userModel.Email,
                CreatedAt = userModel.CreatedAt
            };
        }

        public static AppUser ToUserFromCreateDTO(this CreateUserRequestDto userDto)
        {
            return new AppUser
            {
                Email = userDto.Email,
                PasswordHash = userDto.PasswordHash
            };
        }
    }
}
