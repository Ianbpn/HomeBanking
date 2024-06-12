using HomeBanking.DTOs;
using HomeBanking.Enums;
using HomeBanking.Exceptions;
using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using HomeBanking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;
        private readonly ICardService _cardService;
        private readonly IClientsService _clientsService;
        private readonly IAuthService _authService;
        public ClientsController(IAccountsService accountsService, ICardService cardService , IClientsService clientsService, IAuthService authService)
        {
            _clientsService = clientsService;
            _accountsService = accountsService;
            _cardService = cardService;
            _authService = authService;
        }

        //[HttpGet]
        //public IActionResult Hello()
        //{
        //    return Ok("Buenos dias querido Mundo!!!");
        //}

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetAllClients()
        {
            try
            {
                var clients = _clientsService.GetAllClients();
                return StatusCode(StatusCodes.Status200OK, clients);
            }
            catch (CustomException ex)
            {

                return StatusCode(ex.statusCode, ex.message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetClientById(long id)
        {
            try
            {
                var client = _clientsService.GetClientById(id);
                return Ok(client);
            }
            catch (CustomException ex)
            {

                return StatusCode(ex.statusCode, ex.message);
            }
        }
        [HttpGet("current")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : null;
                if (email.IsNullOrEmpty())
                {
                    return StatusCode(403, "User not Found");
                }
                ClientDTO client= _clientsService.FindClientByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "User not Found");
                }
                return Ok(client);
            }
            catch (CustomException ex)
            {

                return StatusCode(ex.statusCode, ex.message);
            }
        }

        [HttpPost]
        public IActionResult NewClient([FromBody] NewClientDTO newClientDTO)
        {
            try
            {
                if(!_clientsService.VerifyNewClientData(newClientDTO))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Missing fields");
                }
                if (_clientsService.VerifyIfEmailExists(newClientDTO.Email))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Email is already in use");
                }
                var newClient=_clientsService.NewClient(newClientDTO);
                return Ok(newClient);
            }
            catch (CustomException ex)
            {

                return StatusCode(ex.statusCode, ex.message);
            }
        }
        [HttpPost("current/accounts")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult newAccount()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : null;
                if (email.IsNullOrEmpty())
                {
                    return StatusCode(403, "User not Found");
                }
                var currentClient = _clientsService.FindClientByEmail(email);

                if (currentClient == null)
                {
                    return StatusCode(403, "User not Found");
                }

                if (_accountsService.MaxAccountsReached(currentClient.Accounts.ToList()))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "The limit of accounts has been reached");
                }
                long currentClientId = currentClient.Id;

                Account newAccount = _accountsService.newAccount(currentClientId);
                
                return StatusCode(StatusCodes.Status201Created, newAccount);

            }
            catch (CustomException ex)
            {

                return StatusCode(ex.statusCode,ex.message) ;
            }
        }

        [HttpGet("current/accounts")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetCurrentUserAccounts()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : null;
                if (email.IsNullOrEmpty())
                {
                    return StatusCode(403, "User not Found");
                }
                var client = _clientsService.FindClientByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "User not Found");
                }
                var accounts = _accountsService.GetAccountsByClient(client.Id);
                return StatusCode(StatusCodes.Status200OK, accounts);
            }
            catch (CustomException ex)
            {

                return StatusCode(ex.statusCode, ex.message);
            }
        }
        [HttpPost ("current/cards")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult NewCard([FromBody] NewCardDTO newCardDTO)
        {
            if (newCardDTO.Type.IsNullOrEmpty() || newCardDTO.Color.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Missing fields");
            }
            string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : null;
            if (email.IsNullOrEmpty())
            {
                return StatusCode(403, "User not Found");
            }
            var currentClient = _clientsService.FindClientByEmail(email);

            if (currentClient == null)
            {
                return StatusCode(403, "User not Found");
            }
            var newCard = _cardService.NewCardForClient(newCardDTO,currentClient.Id);
                return Ok(newCard);
            
        }

        [HttpGet("current/cards")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetCurrentUserCards()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : null;
                if (email == null)
                {
                    return StatusCode(403, "User not Found");
                }
                var client = _clientsService.FindClientByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "User not Found");
                }
                var cards = _cardService.GetCardsFromClient(client.Id);
                return StatusCode(StatusCodes.Status200OK, cards);
            }
            catch (CustomException ex)
            {

                return StatusCode(ex.statusCode, ex.message);
            }
        }
    }
}
