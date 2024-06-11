using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;

namespace HomeBanking.Services.Implementations
{
    public class ClientLoanService : IClientLoanService
    {
        public readonly IClientLoanRepository _clientLoanRepository;

        public ClientLoanService(IClientLoanRepository clientLoanRepository)
        {
            _clientLoanRepository = clientLoanRepository;
        }

        public void AddClientLoan(ClientLoan clientLoan)
        {
            _clientLoanRepository.save(clientLoan);
        }
    }
}
