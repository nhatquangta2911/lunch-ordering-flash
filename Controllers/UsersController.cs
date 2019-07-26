using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Interfaces;
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
        private readonly IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // [AllowAnonymous]
        // [HttpPost("auth")]
        // public async Task<IActionResult> Authenticate([FromBody] UserAuthDto userParam)
        // {
        //     var token = await _userRepository.AuthenticateAsync(userParam.Username, userParam.Password);
        //     if(token == null)
        //         return BadRequest(new { message = "Username or Password is incorrect" });
        //     return Ok(token);
        // }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<UserResponseDto>> Get([FromQuery] string id)
        {
            return Ok(await _userRepository.GetById(id));
        }

        [AllowAnonymous]
        [Route("GetAllUsers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAll();
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
      public async Task<IActionResult> CreateAsync([FromBody] UserRegisterDto user)
      {
         user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
         var token = await _userRepository.CreateAsync(user);

         await _unitOfWork.Commit();

         return Ok(token);
      }

      // [HttpPut]
      // public async Task<IActionResult> Update([FromQuery] string id, User userIn)
      // {
      //     var user = await _userRepository.Get(id);
      //     if(user == null)
      //         return NotFound();
      //     userIn.Password = BCrypt.Net.BCrypt.HashPassword(userIn.Password);
      //     await _userRepository.UpdateAsync(id, userIn);
      //     return NoContent();
      // }

      // [HttpDelete]
      // public async Task<IActionResult> Delete(string id)
      // {
      //     var user = await _userRepository.Get(id);
      //     if(user == null)
      //         return NotFound();
      //     await _userRepository.DeleteAsync(id);
      //     return NoContent();
      // }

   }

}