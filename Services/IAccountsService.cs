using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface IAccountsService
    {
        string GenerateUniqueNumber();
        Account FindAccountByNumber(string number);
        IEnumerable<Account> GetAccountsByClient(long id);
        void Save(Account account);
    }
}
