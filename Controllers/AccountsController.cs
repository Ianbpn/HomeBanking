using HomeBanking.DTOs;
using HomeBanking.Repositories.Implementations;
using HomeBanking.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;
        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService; 
        }

        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            try
            {
                var accounts = _accountsService.GetAllAccounts();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetAccount(int id)
        {
            try
            {
                var account = _accountsService.GetAccountById(id);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
