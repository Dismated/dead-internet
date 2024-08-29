using LLMForum.Server.Data;
using LLMForum.Server.Dtos.AppUser;
using LLMForum.Server.Interfaces;
using LLMForum.Server.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace LLMForum.Server.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AppUserController(ApplicationDBContext context, IAppUserRepository userRepo)
        : ControllerBase
    {
        private readonly ApplicationDBContext _context = context;
        private readonly IAppUserRepository _userRepo = userRepo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepo.GetAllAsync();
            var userDtos = users.Select(s => s.ToAppUserDto());

            return Ok(userDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.ToAppUserDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppUserRequestDto userDto)
        {
            var userModel = userDto.ToAppUserFromCreateDTO();
            await _userRepo.CreateAsync(userModel);
            return CreatedAtAction(
                nameof(GetById),
                new { id = userModel.Id },
                userModel.ToAppUserDto()
            );
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] string id,
            [FromBody] UpdateAppUserRequestDto updateDto
        )
        {
            var userModel = await _userRepo.UpdateAsync(id, updateDto);
            if (userModel == null)
            {
                return NotFound();
            }

            return Ok(userModel.ToAppUserDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var userModel = await _userRepo.DeleteAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
