using HomeBanking.DTOs;
using HomeBanking.Repositories.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            try
            {
                var accounts = _accountRepository.GetAllAccount();
                var accountsDTO = accounts.Select(a => new AccountDTO(a)).ToList();
                return Ok(accountsDTO);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  e.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetAccount(int id)
        {
            try
            {
                var account= _accountRepository.FindById(id);
                var accountDTO = new AccountDTO(account);
                return Ok(accountDTO);
            }
            catch (Exception e) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  e.Message);
            }
        }
    }
}
