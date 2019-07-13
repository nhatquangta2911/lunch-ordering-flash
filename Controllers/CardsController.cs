using System.Collections.Generic;
using CourseApi.Entities;
using CourseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly CardService _cardService;
        public CardsController(CardService cardService)
        {
            _cardService = cardService;
        }
        /// <summary>
        /// Gets a list of Card items.
        /// </summary>
        [HttpGet]
        public ActionResult<List<Card>> Get() =>
            _cardService.Get();

        /// <summary>
        /// Gets a card item by its id.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id:length(24)}", Name="GetCard")]
        public ActionResult<Card> Get(string id)
        {
            var card = _cardService.Get(id);
            if(card == null)
                return NotFound();
            return card;
        }

        // [HttpGet("latest")]
        // public ActionResult<Card> GetLatest()
        // {
        //     var card = _cardService.GetLatest();
        //     if(card == null)
        //         return NotFound();
        //     return card;
        // }

        /// <summary>
        /// Creates a card item.
        /// </summary>
        /// <remarks>
        /// Sample request: 
        ///
        ///     POST /cards
        ///     {
        ///         "EnglishTitle": "Hello",
        ///         "VietnameseTitle": "Xin Chao"  
        ///     }
        ///
        /// </remarks>
        /// <param name="card"></param>
        /// <returns>A newly created card</returns>
        /// <response code="201">Returns the newly create card</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
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