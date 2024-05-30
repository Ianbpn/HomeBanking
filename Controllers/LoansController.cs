using HomeBanking.DTOs;
using HomeBanking.Repositories.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;

        public LoansController(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
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
    }
}