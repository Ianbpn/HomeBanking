using HomeBanking.Models;

namespace HomeBanking.Repositories.Implementations
{
    public interface IClientLoanRepository
    {
        void save(ClientLoan clientLoan);
    }
}
