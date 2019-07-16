using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
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
        private readonly OrderService _orderService;
        private readonly MenuService _menuService;
        private readonly UserService _userService;
        private readonly DailyChoiceService _dailyChoiceService;
        private readonly IMapper _mapper;
        const double DUE_HOUR = 0.2;

        public OrdersController(OrderService orderService, MenuService menuService, UserService userService, DailyChoiceService dailyChoiceService, IMapper mapper)
        {
            _orderService = orderService;
            _menuService = menuService;
            _userService = userService;
            _dailyChoiceService = dailyChoiceService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("getAllOrders")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get()
        {
            return await _orderService.Get();
        }

        [AllowAnonymous]
        [HttpGet]
        //TODO:
        public async Task<ActionResult<OrderResponseDto>> Get([FromQuery] string id)
        {
            var order = await _orderService.Get(id);
            var response = new OrderResponseDto {
                Id = order.Id,
                DateOrdered = order.DateOrdered,
                User = _mapper.Map<UserResponseDto>(await _userService.Get(order.UserId)),
                Menu = await _menuService.Get(order.MenuId),
                DailyChoice = await _dailyChoiceService.Get(order.DailyChoiceId)
            };
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            var dailyChoice =  await _dailyChoiceService.Get(order.DailyChoiceId);
            if(dailyChoice.MenuIds.Contains((await _menuService.Get(order.MenuId)).Id) == false)
                return BadRequest("This menu is not a choice today.");
                
            var now = DateTime.UtcNow;
            if((now - dailyChoice.dateCreated).TotalHours >= DUE_HOUR)
                return BadRequest("Overdue.");

            dailyChoice.amountOfChoices += 1;
            await _dailyChoiceService.Update(dailyChoice.Id, dailyChoice);
            return Ok(await _orderService.Create(order));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] string id, Order orderIn)
        {
            var order = await _orderService.Get(id);
            if(order == null)
                return NotFound();
            await _orderService.Update(id, orderIn);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var order = await _orderService.Get(id);
            if(order == null)
                return NotFound();
            await _orderService.Delete(id);
            return NoContent();
        }

    }
}