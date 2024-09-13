using LLMForum.Server.Dtos.AppUser;
using LLMForum.Server.Models;

namespace LLMForum.Server.Mappers
{
    public static class AppUserMapper
    {
        public static AppUserDto ToAppUserDto(this AppUser appUserModel)
        {
            return new AppUserDto
            {
                Id = appUserModel.Id,
                Email = appUserModel.Email,
                CreatedAt = appUserModel.CreatedAt,
            };
        }

        public static AppUser ToAppUserFromCreateDTO(this CreateAppUserRequestDto appUserDto)
        {
            return new AppUser { Email = appUserDto.Email, PasswordHash = appUserDto.PasswordHash };
        }
    }
}
