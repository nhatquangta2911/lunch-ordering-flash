using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Services.Dishes;
using CourseApi.Services.Dishes.Dtos;
using CourseApi.Services.Menus;
using CourseApi.Services.Menus.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly MenuService _menuService;
        private readonly DishService _dishService;
        private readonly IMapper _mapper;

        public MenusController(MenuService menuService, DishService dishService, IMapper mapper)
        {
            _menuService = menuService;
            _dishService = dishService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("GetAllMenus")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> Get()
        {
            return await _menuService.Get();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<Menu>> Get([FromQuery] string id)
        {
            var menu = await _menuService.Get(id);    
            HashSet<Dish> dishes = new HashSet<Dish>();
            foreach(var dishId in menu.DishIds)
            {
                dishes.Add(await _dishService.Get(dishId));
            }
             
            var response = new MenuResponseDto {
                Id = menu.Id,
                Name = menu.Name,
                Dishes = dishes
            };
            return Ok(response);   
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Menu menu)
        {
            menu.DishIds = new HashSet<string>(menu.DishIds);
            return Ok(await _menuService.Create(menu));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] string id, Menu menuIn)
        {
            var menu = await _menuService.Get(id);
            if(menu == null)
                return NotFound();
            await _menuService.Update(id, menuIn);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var menu = await _menuService.Get(id);
            if(menu == null)
                return NotFound();
            await _menuService.Delete(id);
            return NoContent();
        }
    }
}