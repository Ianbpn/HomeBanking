using HomeBanking.DTOs;
using HomeBanking.Exceptions;
using HomeBanking.Models;
using HomeBanking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly IClientsService _clientsService;

        public LoansController(ILoanService loanService, IClientsService clientsService)
        {
            _loanService = loanService;
            _clientsService = clientsService;
        }

        [HttpGet]
        public IActionResult GetAllLoans()
        {
            try
            {
                var loansDTO = _loanService.GetAllLoans();
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
                    var loan = _loanService.FindLoanById(id);
                    return Ok(loan);

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
                ClientDTO client = _clientsService.FindClientByEmail(email);
                if (client == null)
                {
                    return StatusCode(403, "User not Found");
                }
                ClientLoan newClientLoan = _loanService.LoanRequest(loanApplicationDTO,client);

                return Ok(newClientLoan);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.statusCode, ex.message);
            }
        }
    }
}