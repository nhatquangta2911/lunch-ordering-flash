using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Services.DailyChoices;
using CourseApi.Services.DailyChoices.Dtos;
using CourseApi.Services.Menus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
   public class DailyChoicesController : ControllerBase
    {
        private readonly DailyChoiceService _dailyChoiceService;
        private readonly MenuService _menuService;
        private readonly IMapper _mapper;

        public DailyChoicesController(DailyChoiceService dailyChoiceService, MenuService menuService, IMapper mapper)
        {
            _dailyChoiceService = dailyChoiceService;
            _menuService = menuService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("GetAllChoices")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyChoice>>> Get()
        {
            return await _dailyChoiceService.Get();
        }
    
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<DailyChoice>> Get([FromQuery] string id)
        {
            var dailyChoice = await _dailyChoiceService.Get(id);
            HashSet<Menu> menus = new HashSet<Menu>();
            foreach(var menuId in dailyChoice.MenuIds)
            {
                menus.Add(await _menuService.Get(menuId));
            }
            var response = new DailyChoiceDto {
                Id = dailyChoice.Id,
                Name = dailyChoice.Name,
                Menus = menus
            };
            return Ok(response);
            
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DailyChoiceForAddingDto dailyChoiceIn)
        {
            dailyChoiceIn.MenuIds = new HashSet<string>(dailyChoiceIn.MenuIds);
            return Ok(await _dailyChoiceService.Create(dailyChoiceIn));
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] string id, DailyChoice dailyChoiceIn)
        {
            var dailyChoice = await _dailyChoiceService.Get(id);
            if(dailyChoice == null)
                return NotFound();
            await _dailyChoiceService.Update(id, dailyChoiceIn);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var dailyChoice = await _dailyChoiceService.Get(id);
            if(dailyChoice == null)
                return NotFound();
            await _dailyChoiceService.Delete(id);
            return NoContent();
        }

    }
}