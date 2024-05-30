
using HomeBanking.Database.Models;

namespace HomeBanking.Repositories.Implementations
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetAllTransaction();
        void Save(Transaction transaction);
        Transaction FindById(long id);
    }
}
