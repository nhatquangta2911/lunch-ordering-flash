using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApi.Entities;
using CourseApi.Services.Dishes;
using CourseApi.Services.Dishes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    { 
        private readonly DishService _dishService;

        public DishesController(DishService dishService)
        {
            _dishService = dishService;
        }

        [AllowAnonymous]
        [Route("GetAllDishes")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> Get() {
            return await _dishService.Get();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DishForAddingDto dishForAdding)
        {
            return Ok(await _dishService.Create(dishForAdding));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] string id, Dish dishIn)
        {
            var dish = await _dishService.Get(id);
            if(dish == null)
                return NotFound();
            await _dishService.Update(id, dishIn);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var dish = await _dishService.Get(id);
            if(dish == null)
                return NotFound();
            await _dishService.Delete(id);
            return NoContent();
        }

    }
}