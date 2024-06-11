using HomeBanking.DTOs;
using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using HomeBanking.Services;
using HomeBanking.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ILoanService _loanService;
        private readonly iClientsService _clientsService;
        private readonly IAccountsService _accountsService;
        private readonly IClientLoanService _clientLoanService;

        public LoansController(ILoanRepository loanRepository, ILoanService loanService, iClientsService clientsService, IAccountsService accountsService, IClientLoanService clientLoanService)
        {
            _loanRepository = loanRepository;
            _loanService = loanService;
            _clientsService = clientsService;
            _accountsService = accountsService;
            _clientLoanService = clientLoanService;
        }

        [HttpGet]
        public IActionResult GetAllLoans()
        {
            try
            {
                var loans = _loanRepository.GetAllLoans();
                var loansDTO = loans.Select(loans => new LoanDTO(loans)).ToList();
                return Ok(loansDTO);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetLoanById(long id)
        {
            {
                try
                {
                    var loan = _loanRepository.GetLoanById(id);
                    var loanDTO = new LoanDTO(loan);
                    return Ok(loanDTO);

                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }
            }
        }
        [HttpPost]
        [Authorize(Policy ="ClientOnly")]
        public IActionResult AskLoan([FromBody] LoanApplicationDTO loanApplicationDTO)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : null;
                if (email == null)
                {
                    return StatusCode(403, "User not Found");
                }
                Client client = _clientsService.ReturnCurrentClient(email);
                if (client == null)
                {
                    return StatusCode(403, "User not Found");
                }
                ClientLoan newClientLoan = _loanService.LoanRequest(loanApplicationDTO,client);

                return Ok(newClientLoan);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}