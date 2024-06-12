using HomeBanking.DTOs;
using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface IAccountsService
    {
        string GenerateUniqueNumber();
        Account FindAccountByNumber(string number);
        IEnumerable<AccountDTO> GetAccountsByClient(long id);
        void Save(Account account);
        List<AccountDTO> GetAllAccounts();
        AccountDTO GetAccountById(long id);
        bool MaxAccountsReached(List<AccountClientDTO> accounts);
        public Account newAccount(long clientId);
    }
}
