using HomeBanking.DTOs;
using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using System.Security.Cryptography;

namespace HomeBanking.Services.Implementations
{
    public class AccountsService : IAccountsService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountsService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Account FindAccountByNumber(string number)
        {
            return _accountRepository.FindByNumber(number);
        }
        public string GenerateUniqueNumber()
        {
            string NumberAccountRandom;
            do
            {
                NumberAccountRandom = "VIN-" + RandomNumberGenerator.GetInt32(0, 99999999);
            } while (_accountRepository.FindByNumber(NumberAccountRandom) != null);
            return NumberAccountRandom;
        }

        public AccountDTO GetAccountById(long id)
        {
            var account = _accountRepository.FindById(id);
            var accountDTO = new AccountDTO(account);
            return accountDTO;
        }

        public IEnumerable<AccountDTO> GetAccountsByClient(long id)
        {
            var account = _accountRepository.GetAccountsByClient(id);
            var accounts = account.Select(a=> new AccountDTO(a)).ToList();
            return accounts;
        }

        public List<AccountDTO> GetAllAccounts()
        {
            var accounts = _accountRepository.GetAllAccount();
            var accountsDTO = accounts.Select(a => new AccountDTO(a)).ToList();
            return accountsDTO;
        }

        public void Save(Account account)
        {
            _accountRepository.Save(account);
        }

        public bool MaxAccountsReached(List<AccountClientDTO> accounts)
        {
            //Si la cantidad de cuentas es 3 o mas, significa que llego al limite
            if(accounts.Count()>=3)
            {
                return true;
            }
            return false;
        }

        public Account newAccount(long clientId)
        {
            Account newAccount = new ()
            {
                Number = GenerateUniqueNumber().ToString(),
                CreationDate = DateTime.Now,
                Balance = 0,
                ClientId = clientId
            };
            Save(newAccount);
            return newAccount;
        }
    }

}


        //public string GenerateUniqueNumber()
        //{
        //    try
        //    {
        //        var lowerBound =0;
        //        var upperBound = 100000000;
        //        string uniqueNumber = string.Empty;
        //        bool isUnique= false;
        //        while (!isUnique)
        //        {
        //            string RngNumber = Convert.ToString(RandomNumberGenerator.GetInt32(lowerBound, upperBound));
        //            uniqueNumber = "VIN"+RngNumber;
        //            Account account = (Account)_accountRepository.GetAccountsByNumber(RngNumber);
        //            if (account== null){
        //                isUnique = true;
        //            }
        //        }
        //        return uniqueNumber;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception();
        //    }
        //}