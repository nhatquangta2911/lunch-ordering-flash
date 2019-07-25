using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Interfaces;
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
        private readonly IMenuRepository _menuRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenusController(IMenuRepository menuRepository, IDishRepository dishRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _dishRepository = dishRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> Get()
        {
            var menus = await _menuRepository.GetAll(); 
            return Ok(menus);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> Get(string id)
        {
            var menu = await _menuRepository.GetById(id);    
            HashSet<Dish> dishes = new HashSet<Dish>();
            foreach(var dishId in menu.DishIds)
            {
                dishes.Add(await _dishRepository.GetById(dishId));
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
            _menuRepository.Add(menu);

            var testMenu = await _menuRepository.GetById(menu.Id);

            await _unitOfWork.Commit();

            testMenu = await _menuRepository.GetById(menu.Id);

            return Ok(testMenu);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Menu menuIn)
        {
            var menu = await _menuRepository.GetById(id);
            if(menu == null)
                return NotFound();
            _menuRepository.Update(menuIn);

            await _unitOfWork.Commit();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var menu = await _menuRepository.GetById(id);
            if(menu == null)
                return NotFound();

            _menuRepository.Remove(id);

            var testMenu = await _menuRepository.GetById(id);

            await _unitOfWork.Commit();

            testMenu = await _menuRepository.GetById(id);

            return NoContent();
        }
    }
}