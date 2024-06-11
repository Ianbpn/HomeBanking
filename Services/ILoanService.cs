using HomeBanking.DTOs;
using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface ILoanService
    {
        ClientLoan LoanRequest(LoanApplicationDTO loanApplicationDTO, Client client);
        Loan FindLoanById(long id);
    }
}
