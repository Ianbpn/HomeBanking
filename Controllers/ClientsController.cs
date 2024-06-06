using HomeBanking.DTOs;
using HomeBanking.Enums;
using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using HomeBanking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountsService _accountsService;
        private readonly ICardRepository _cardRepository;
        private readonly ICardService _cardService;
        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository, IAccountsService accountsService, ICardRepository cardRepository,ICardService cardService)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _accountsService = accountsService;
            _cardRepository = cardRepository;  
            _cardService = cardService;
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
                var clients = _clientRepository.GetAllClients();
                var clientsDTO = clients.Select(c => new ClientDTO(c)).ToList();
                return StatusCode(StatusCodes.Status200OK, clientsDTO);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetClientById(long id)
        {
            try
            {
                var client = _clientRepository.FindById(id);
                var clientDTO = new ClientDTO(client);
                return Ok(clientDTO);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
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
                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "User not Found");
                }
                return Ok(new ClientDTO(client));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult NewClient([FromBody] NewClientDTO newClientDTO)
        {
            try
            {
                if(newClientDTO.FirstName.IsNullOrEmpty() || newClientDTO.LastName.IsNullOrEmpty() || newClientDTO.Password.IsNullOrEmpty() || newClientDTO.Email.IsNullOrEmpty())
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Missing fields");
                }
                Client client = _clientRepository.FindByEmail(newClientDTO.Email);
                if (client != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Email is already in use");
                }
                Client newClient = new Client
                {
                    FirstName = newClientDTO.FirstName,
                    LastName = newClientDTO.LastName,
                    Password = newClientDTO.Password,
                    Email = newClientDTO.Email
                };
                _clientRepository.Save(newClient);
                return Ok(newClient);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
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
                Client currentClient = _clientRepository.FindByEmail(email);

                if (currentClient == null)
                {
                    return StatusCode(403, "User not Found");
                }

                if (currentClient.Accounts.Count() >= 3)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "The limit of accounts has been reached");
                }
                long currentClientId = currentClient.Id;

                Account newAccount = new Account
                {
                    Number = _accountsService.GenerateUniqueNumber().ToString(),
                    CreationDate = DateTime.Now,
                    Balance = 0,
                    ClientId = currentClient.Id
                };
                _accountRepository.Save(newAccount);
                return StatusCode(201, "Account created");

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message); ;
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
                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "User not Found");
                }
                var accounts = _accountRepository.GetAccountsByClient(client.Id);
                var accountDTO = accounts.Select(a => new AccountClientDTO (a)).ToList();
                return StatusCode(StatusCodes.Status200OK, accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

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
            Client currentClient = _clientRepository.FindByEmail(email);

            if (currentClient == null)
            {
                return StatusCode(403, "User not Found");
            }

            var cards = _cardRepository.FindCardByTypeFromClient(newCardDTO.Type, currentClient.Id);
            if (cards.Count() >= 3)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "El limite de tarjetas se alcanzo");
            }
            else
            {
                if(cards.Any(c => c.Color == newCardDTO.Color))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Ya hay de este color");
                }
                int newCvv = _cardService.GenerateCVV();
                string newNumber = _cardService.GenerateUniqueNumber();
                string newType = newCardDTO.Type.ToString().ToUpper();
                string newColor = newCardDTO.Color.ToString().ToUpper();

                Card newCard = new Card()
                {
                    ClientId = currentClient.Id,
                    CardHolder = currentClient.FirstName + " " + currentClient.LastName,
                    Type = newType,
                    Color = newColor,
                    Number = newNumber,
                    Cvv = newCvv,
                    FromDate = DateTime.Now,
                    ThruDate = DateTime.Now.AddYears(5),
                };
                _cardRepository.Save(newCard);
                return Ok(newCard);
            }
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
                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "User not Found");
                }
                var cards = _cardRepository.GetCardsFromClient(client.Id);
                var cardDTO= cards.Select(card=>new CardDTO(card)).ToList();
                return StatusCode(StatusCodes.Status200OK, cardDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }
    }
}
