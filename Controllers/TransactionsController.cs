using HomeBanking.DTOs;
using HomeBanking.Repositories.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
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
    }
}
