using LLMForum.Server.Dtos.User;
using LLMForum.Server.Models;

namespace LLMForum.Server.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToUserDto(this User userModel)
        {
            return new UserDto
            {
                Id = userModel.Id,
                Username = userModel.Username,
                Email = userModel.Email,
                CreatedAt = userModel.CreatedAt
            };
        }

        public static User ToUserFromCreateDTO(this CreateUserRequestDto userDto)
        {

            return new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = userDto.PasswordHash
            };
        }
    }
}
