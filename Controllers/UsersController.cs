using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Interfaces;
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

      [AllowAnonymous]
      [HttpPost("auth")]
      public async Task<IActionResult> Authenticate([FromBody] UserAuthDto userParam)
      {
         var token = await _userRepository.AuthenticateAsync(userParam.Username, userParam.Password);
         if (token == null)
            return BadRequest(new { message = "Username or Password is incorrect" });
         return Ok(token);
      }

      [AllowAnonymous]
      [HttpGet("{id}")]
      public async Task<ActionResult<UserResponseDto>> Get(string id)
      {
         var user = await _userRepository.GetById(id);
         return Ok(_mapper.Map<UserResponseDto>(user));
      }

      [AllowAnonymous]
      [HttpGet]
      public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
      {
         var users = await _userRepository.GetAll();
         var response = users.Select(user => _mapper.Map<UserResponseDto>(user));
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

      [AllowAnonymous]
      [HttpPut("{id}")]
      public async Task<IActionResult> Update(string id, User userIn)
      {
          var user = await _userRepository.GetById(id);
          if(user == null)
              return NotFound();

          userIn.Password = BCrypt.Net.BCrypt.HashPassword(userIn.Password);

          _userRepository.Update(userIn);

          await _unitOfWork.Commit();

          return NoContent();
      }

      [AllowAnonymous]
      [HttpDelete("{id}")]
      public async Task<IActionResult> Delete(string id)
      {
          var user = await _userRepository.GetById(id);
          if(user == null)
              return NotFound();

          _userRepository.Remove(id);

          var testUser = await _userRepository.GetById(id);
           
          await _unitOfWork.Commit(); 

          testUser = await _userRepository.GetById(id);

          return NoContent();
      }

   }

}