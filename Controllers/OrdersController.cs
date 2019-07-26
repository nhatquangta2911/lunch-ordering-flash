using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Interfaces;
using CourseApi.Services.DailyChoices;
using CourseApi.Services.Menus;
using CourseApi.Services.Orders;
using CourseApi.Services.Orders.Dtos;
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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDailyChoiceRepository _dailyChoiceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        const double DUE_HOUR = 18;

        public OrdersController(IOrderRepository orderRepository, IMenuRepository menuRepository, IUserRepository userRepository, IDailyChoiceRepository dailyChoiceRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _menuRepository = menuRepository;
            _userRepository = userRepository;
            _dailyChoiceRepository = dailyChoiceRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get()
        {
            var orders = await _orderRepository.GetAll();
            return Ok(orders);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDto>> Get(string id)
        {
            var order = await _orderRepository.GetById(id);
            var response = new OrderResponseDto {
                Id = order.Id,
                DateOrdered = order.DateOrdered,
                User = _mapper.Map<UserResponseDto>(await _userRepository.GetById(order.UserId)),
                Menu = await _menuRepository.GetById(order.MenuId),
                DailyChoice = await _dailyChoiceRepository.GetById(order.DailyChoiceId)
            };
            return Ok(response);
        }

        [AllowAnonymous]
        [Route("{id}/stats")]
        [HttpGet]
        public async Task<ActionResult<Object>> GetByDailyChoice(string id) 
        {
            var dailyChoice = await _dailyChoiceRepository.GetById(id);
            var orders = await _orderRepository.GetOrdersByDailyChoice(id);

            var response = new Dictionary<string, IList>();
            foreach (var menuId in dailyChoice.MenuIds)
            {
                response[menuId] = orders.Where(order => order.MenuId == menuId).ToList();
            }
            return response;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            // Check Menu is whether in the choices
            var dailyChoice =  await _dailyChoiceRepository.GetById(order.DailyChoiceId);
            var menu = await _menuRepository.GetById(order.MenuId);
            if(dailyChoice.MenuIds.Contains(menu.Id) == false)
                return BadRequest("This menu is not a choice today.");

            // Check whether the order is overdue or not 
            var now = DateTime.UtcNow;
            if((now - dailyChoice.dateCreated).TotalHours >= DUE_HOUR)
                return BadRequest("Overdue.");
            
            // Check whether users already ordered or not
            var byUserOrders = await _orderRepository.GetOrdersByUser(order.UserId);
            if(byUserOrders.Find(_ => _.DailyChoiceId == order.DailyChoiceId) != null)
                return BadRequest("You have already ordered today.");

            // Increase the amount of order into 1
            dailyChoice.amountOfChoices += 1;
            _dailyChoiceRepository.Update(dailyChoice);

            _orderRepository.Add(order);

            await _unitOfWork.Commit();

            return Ok(order);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Order orderIn)
        {
            var order = await _orderRepository.GetById(id);
            if(order == null)
                return NotFound();

            _orderRepository.Update(orderIn);

            await _unitOfWork.Commit();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var order = await _orderRepository.GetById(id);
            if(order == null)
                return NotFound();
            
            var dailyChoice = await _dailyChoiceRepository.GetById(order.DailyChoiceId);


            _orderRepository.Remove(id);

            dailyChoice.amountOfChoices -= 1;
            _dailyChoiceRepository.Update(dailyChoice);

            var testOrder = await _orderRepository.GetById(id);

            await _unitOfWork.Commit();

            testOrder = await _orderRepository.GetById(id);

            return NoContent();
        }

    }
}