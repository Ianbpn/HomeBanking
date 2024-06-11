using HomeBanking.DTOs;
using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using HomeBanking.Services;
using HomeBanking.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionsService _transactionsService;
        private readonly iClientsService _clientsService;
        private readonly IAccountsService _accountsService;

        public TransactionsController(ITransactionRepository transactionRepository, ITransactionsService transactionsService,
            iClientsService clientsService,IAccountsService accountsService)
        {
            _transactionRepository = transactionRepository;
            _transactionsService = transactionsService;
            _clientsService = clientsService;
            _accountsService = accountsService;
        }

        [HttpGet]
        public IActionResult GetAllTransactions()
        {
            try
            {
                var transactions = _transactionRepository.GetAllTransaction();
                var transactionsDTO = transactions.Select(t=> new TransactionDTO(t)).ToList();
                return Ok(transactionsDTO);
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
                var transaction = _transactionRepository.FindById(id);
                var transactionDTO = new TransactionDTO(transaction);
                return Ok(transactionDTO);
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
                var client = _clientsService.ReturnCurrentClient(email);
                if (client == null)
                {
                    return StatusCode(403, "User not Found");
                }
                _transactionsService.AccountToAccountTransaction(client.Id, newTransactionDTO);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
