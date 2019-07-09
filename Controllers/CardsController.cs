using System.Collections.Generic;
using CourseApi.Models;
using CourseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly CardService _cardService;
        public CardsController(CardService cardService)
        {
            _cardService = cardService;
        }
        [HttpGet]
        public ActionResult<List<Card>> Get() =>
            _cardService.Get();

        [HttpGet("{id:length(24)}", Name="GetCard")]
        public ActionResult<Card> Get(string id)
        {
            var card = _cardService.Get(id);
            if(card == null)
                return NotFound();
            return card;
        }
        [HttpPost]
        public ActionResult<Card> Create(Card card)
        {
            _cardService.Create(card);
            return CreatedAtRoute("GetCard", new { id = card.Id.ToString() }, card);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Card cardIn)
        {
            var card = _cardService.Get(id);
            if(card == null) 
                return NotFound();
            _cardService.Update(id, cardIn);
            return NoContent();
        }
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id) 
        {
            var card = _cardService.Get(id);
            if(card == null) 
                return NotFound();
            _cardService.Delete(id);
            return NoContent();
        }
    }
}