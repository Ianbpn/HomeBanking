using HomeBanking.Models;

namespace HomeBanking.Repositories.Implementations
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccount();
        void Save(Account account);
        Account FindById(long id);
    }
}
