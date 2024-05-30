using HomeBanking.Database.Models;

namespace HomeBanking.Repositories.Implementations
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Transaction> GetAllTransaction()
        {
            return FindAll();
        }
        public Transaction FindById(long id)
        {
            return FindByCondition(t => t.Id == id)
                .FirstOrDefault();
        }

        public void Save(Transaction transaction)
        {
            Create(transaction);
            SaveChanges();
        }
    }
}
