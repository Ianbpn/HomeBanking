using HomeBanking.DTOs;
using HomeBanking.Repositories.Implementations;
using HomeBanking.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        public IActionResult GetAllCards()
        {
            try
            {
                var cards = _cardService.GetAllCards();
                return StatusCode(StatusCodes.Status200OK, cards);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCardByID(long id)
        {
            try
            {
                var card = _cardService.GetCardById(id);                
                return StatusCode(StatusCodes.Status200OK, card);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
