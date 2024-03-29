using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Interfaces;
using CourseApi.Services.Dishes.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IDishRepository _dishRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public DishesController(IDishRepository dishRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _dishRepository = dishRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> Get()
        {
            var dishes = await _dishRepository.GetAll();
            return Ok(dishes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> Get(string id)
        {
            var dish = await _dishRepository.GetById(id);
            return Ok(dish);
        }

        [Route("PostSimulatingError")]
        [HttpPost]
        public IActionResult PostSimulatingError([FromBody] DishForAddingDto dishIn)
        {
            var dish = _mapper.Map<Dish>(dishIn);
            _dishRepository.Add(dish);
            return BadRequest();
        }
        
        [HttpPost]
        public async Task<ActionResult<Dish>> Create([FromBody] DishForAddingDto dishIn)
        {
            var dish = _mapper.Map<Dish>(dishIn);
            _dishRepository.Add(dish);

            var testDish = await _dishRepository.GetById(dish.Id);

            await _unitOfWork.Commit();

            testDish = await _dishRepository.GetById(dish.Id);

            return Ok(testDish);
        }
    
        [HttpPut("{id}")]
        public async Task<ActionResult<Dish>> Update(string id, [FromBody] Dish dishIn)
        {
            _dishRepository.Update(dishIn);
            await _unitOfWork.Commit();

            return Ok(await _dishRepository.GetById(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            _dishRepository.Remove(id);

            var testDish = await _dishRepository.GetById(id);
            await _unitOfWork.Commit();

            testDish = await _dishRepository.GetById(id);

            return Ok();
        }
    }
}