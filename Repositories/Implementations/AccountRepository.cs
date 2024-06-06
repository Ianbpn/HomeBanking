using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBanking.Repositories.Implementations
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Account FindById(long id)
        {
            return FindByCondition(t=>t.Id==id)
                .Include(t=>t.Transactions)
                .FirstOrDefault();
        }

        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            return FindByCondition(account=>account.ClientId==clientId)
                .Include(account => account.Transactions)
                .ToList();
        }

        public Account GetAccountByNumber(string number)
        {
            return FindByCondition(account=>account.Number==number)
                .FirstOrDefault();
        }

        public IEnumerable<Account> GetAllAccount()
        {
            return FindAll()
                .Include(t=>t.Transactions).ToList();
        }

        public void Save(Account account)
        {
            Create(account);
            SaveChanges();
        }
    }
}
