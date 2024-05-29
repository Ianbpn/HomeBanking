using HomeBanking.Database.Models;

namespace HomeBanking.Repositories.Implementations
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccount();
        void Save(Account client);
        Account FindById(long id);
    }
}
