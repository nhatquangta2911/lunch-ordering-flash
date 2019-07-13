using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Services.Users;
using CourseApi.Services.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
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

        [HttpGet]
        public ActionResult<User> Get([FromQuery] string id)
        {
            return Ok(_userService.Get(id));
        }

        [Route("GetAllUsers")]
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _userService.Get();
            var response = users.Select(user => new UserResponseDto {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name
            });
            return Ok(response);            
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] UserDto userDto) 
        {
            return Ok(_userService.Create(userDto));
        }

    }

}