using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Interfaces;
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
        private readonly IDailyChoiceRepository _dailyChoiceRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DailyChoicesController(IDailyChoiceRepository dailyChoiceRepository, IMenuRepository menuRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _dailyChoiceRepository = dailyChoiceRepository;
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyChoice>>> Get()
        {
            var dailyChoices = await _dailyChoiceRepository.GetAll();
            return Ok(dailyChoices);
        }
    
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<DailyChoice>> Get(string id)
        {
            var dailyChoice = await _dailyChoiceRepository.GetById(id);
          
            HashSet<Menu> menus = new HashSet<Menu>();
            foreach(var menuId in dailyChoice.MenuIds)
            {
                menus.Add(await _menuRepository.GetById(menuId));
            }

            var response = _mapper.Map<DailyChoiceDto>(dailyChoice);
            response.Menus = menus;

            return Ok(response);
            
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DailyChoiceForAddingDto dailyChoiceIn)
        {
            dailyChoiceIn.MenuIds = new HashSet<string>(dailyChoiceIn.MenuIds);

            // Check whether the number of menu is exceed 5 or not
            if(dailyChoiceIn.MenuIds.Count >= 5)
                return BadRequest("A Daily Choice must be less than 5 menus.");

            _dailyChoiceRepository.Add(_mapper.Map<DailyChoice>(dailyChoiceIn));

            await _unitOfWork.Commit();

            return Ok(dailyChoiceIn);
        }


        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, DailyChoice dailyChoiceIn)
        {
            var dailyChoice = await _dailyChoiceRepository.GetById(id);
            if(dailyChoice == null)
                return NotFound();

            _dailyChoiceRepository.Update(dailyChoiceIn);

            await _unitOfWork.Commit();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var dailyChoice = await _dailyChoiceRepository.GetById(id);
            if(dailyChoice == null)
                return NotFound();

            _dailyChoiceRepository.Remove(id);

            var testDailyChoice = await _dailyChoiceRepository.GetById(id);

            await _unitOfWork.Commit();

            testDailyChoice = await _dailyChoiceRepository.GetById(id);

            return NoContent();
        }

    }
}