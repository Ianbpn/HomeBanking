using HomeBanking.Models;

namespace HomeBanking.Repositories.Implementations
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetAllLoans();
        Loan GetLoanById(long id);
        void Save(Loan loan);
    }
}
