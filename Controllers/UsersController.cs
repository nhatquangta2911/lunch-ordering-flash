using System.Collections.Generic;
using System.Linq;
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
        public IActionResult Authenticate([FromBody] UserAuthDto userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);
            if(user == null)
                return BadRequest(new { message = "Username or Password is incorrect" });
            return Ok(user.Token);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<UserResponseDto> Get([FromQuery] string id)
        {
            return Ok(_userService.Get(id));
        }

        [AllowAnonymous]
        [Route("GetAllUsers")]
        [HttpGet]
        public ActionResult<IEnumerable<UserResponseDto>> GetAllUsers()
        {
            var users = _userService.Get();
            var response = users.Select(user => new UserResponseDto {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                IsAdmin = user.IsAdmin, 
                Phone = user.Phone
            });
            return Ok(response);            
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Create([FromBody] UserRegisterDto user) 
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); 
            var response = _userService.Create(user);
            return Ok(response.Token);
        }

        [HttpPut]
        public IActionResult Update([FromQuery] string id, User userIn)
        {
            var user = _userService.Get(id);
            if(user == null)
                return NotFound();
            userIn.Password = BCrypt.Net.BCrypt.HashPassword(userIn.Password);
            _userService.Update(id, userIn);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var user = _userService.Get(id);
            if(user == null)
                return NotFound();
            _userService.Delete(id);
            return NoContent();
        }

    }

}