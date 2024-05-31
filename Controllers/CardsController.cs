using HomeBanking.DTOs;
using HomeBanking.Repositories.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepository;

        public CardsController(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        [HttpGet]
        public IActionResult GetAllCards()
        {
            try
            {
                var cards = _cardRepository.GetAllCards();
                var cardsDTO = cards.Select(cards => new CardDTO(cards)).ToList();
                return StatusCode(StatusCodes.Status200OK, cardsDTO);
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
                var card = _cardRepository.GetCardById(id);
                var cardDTO = new CardDTO(card);
                return StatusCode(StatusCodes.Status200OK, cardDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
