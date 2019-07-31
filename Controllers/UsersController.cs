using System;
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
      private readonly IDailyChoiceRepository _dailyChoiceRepository;
      private readonly IOrderRepository _orderRepository;
      private IUnitOfWork _unitOfWork;
      private readonly IMapper _mapper;
      private const short PAGE_SIZE = 5;

      public UsersController(IUserRepository userRepository, IDailyChoiceRepository dailyChoiceRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper)
      {
         _userRepository = userRepository;
         _dailyChoiceRepository = dailyChoiceRepository;
         _orderRepository = orderRepository;
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
      [HttpGet("overallStatus/{index}/page")]
      public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetOverallStatus(short index)
      {
         var users = await _userRepository.GetAll();
         var todayDailyChoice = await _dailyChoiceRepository.GetToday();
         if((DateTime.UtcNow - todayDailyChoice.dateCreated).TotalHours >= 18)
                return NoContent();

         var orders = await _orderRepository.GetOrdersByDailyChoice(todayDailyChoice.Id);

         var alreadyOrderedUsers = orders.Select(_ => _.UserId);

         var usersResponse = users.Select(user => {
            var userTemp = _mapper.Map<UserResponseDto>(user);
            if(alreadyOrderedUsers.Contains(userTemp.Id))
               userTemp.IsOrdered = true;
            return userTemp;
         }).Skip(PAGE_SIZE * (index - 1))
           .Take(PAGE_SIZE);
         return Ok(usersResponse);
      }

      [AllowAnonymous]
      [HttpGet("{id}")]
      public async Task<ActionResult<UserResponseDto>> Get(string id)
      {
         var user = await _userRepository.GetById(id);
         if(user == null)
            return NotFound("Not Found.");
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
      [HttpGet("{dailyChoiceId}/notOrderedYet")]
      public async Task<ActionResult<List<UserResponseDto>>> GetNotOrderedYetUsers(string dailyChoiceId)
      {
         var orders = await _orderRepository.GetOrdersByDailyChoice(dailyChoiceId);
         var users = await _userRepository.GetAll();
         var allUserIds = users.Select(_ => _.Id).ToList();
         var alreadyOrderedUsers = orders.Select(_ => _.UserId).ToList();

         foreach (var userId in alreadyOrderedUsers)
             allUserIds.Remove(userId);

         var response = new List<UserResponseDto>();
         foreach (var userId in allUserIds)
         {
            var user = await _userRepository.GetById(userId);
            response.Add(_mapper.Map<UserResponseDto>(user));
         }

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