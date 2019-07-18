using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Services.Users;
using CourseApi.Services.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
  [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UsersController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthDto userParam)
        {
            var token = await _userService.AuthenticateAsync(userParam.Username, userParam.Password);
            if(token == null)
                return BadRequest(new { message = "Username or Password is incorrect" });
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<UserResponseDto>> Get([FromQuery] string id)
        {
            return Ok(await _userService.Get(id));
        }

        [AllowAnonymous]
        [Route("GetAllUsers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
        {
            var users = await _userService.Get();
            var response = users.Select(user => new UserResponseDto {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Phone = user.Phone
            });
            return Ok(response);            
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Create([FromBody] UserRegisterDto user) 
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); 
            var token = await _userService.CreateAsync(user);
            return Ok(token);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] string id, User userIn)
        {
            var user = await _userService.Get(id);
            if(user == null)
                return NotFound();
            userIn.Password = BCrypt.Net.BCrypt.HashPassword(userIn.Password);
            await _userService.UpdateAsync(id, userIn);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.Get(id);
            if(user == null)
                return NotFound();
            await _userService.DeleteAsync(id);
            return NoContent();
        }

    }

}