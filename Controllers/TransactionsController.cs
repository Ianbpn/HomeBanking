using HomeBanking.DTOs;
using HomeBanking.Exceptions;
using HomeBanking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;
        private readonly IClientsService _clientsService;
        private readonly IAccountsService _accountsService;

        public TransactionsController( ITransactionsService transactionsService,
            IClientsService clientsService,IAccountsService accountsService)
        {
            _transactionsService = transactionsService;
            _clientsService = clientsService;
            _accountsService = accountsService;
        }

        [HttpGet]
        public IActionResult GetAllTransactions()
        {
            try
            {
                var transactions = _transactionsService.GetAllTransactions();
                return Ok(transactions);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetTransactionsById(long id)
        {
            try
            {
                var transaction = _transactionsService.GetTransactionById(id);
                return Ok(transaction);
            }
            catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpPost]
        [Authorize(Policy ="ClientOnly")]
        public IActionResult NewTransaction([FromBody] NewTransactionDTO newTransactionDTO)
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
                _transactionsService.AccountToAccountTransaction(client.Id, newTransactionDTO);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (CustomException ex)
            {

                return StatusCode(ex.statusCode, ex.message);
            }
        }
    }
 }
