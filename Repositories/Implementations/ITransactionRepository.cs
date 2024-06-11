using HomeBanking.Models;

namespace HomeBanking.Repositories.Implementations
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetAllTransaction();
        void Save(Transaction transaction);
        Transaction FindById(long id);

    }
}
