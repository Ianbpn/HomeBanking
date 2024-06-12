using HomeBanking.DTOs;
using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface ILoanService
    {
        ClientLoan LoanRequest(LoanApplicationDTO loanApplicationDTO, ClientDTO client);
        Loan FindLoanById(long id);
        List<LoanDTO> GetAllLoans();
    }
}
