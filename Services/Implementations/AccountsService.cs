﻿using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using Humanizer;
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
        public string GenerateUniqueNumber()
        {
            string NumberAccountRandom;
            do
            {
                NumberAccountRandom = "VIN-" + RandomNumberGenerator.GetInt32(0, 99999999);
            } while (_accountRepository.FindByNumber(NumberAccountRandom) != null);
            return NumberAccountRandom;
        }
        public IEnumerable<Account> GetAccountsByClient(long id)
        {
            return _accountRepository.GetAccountsByClient(id);
        }

        public void Save(Account account)
        {
            _accountRepository.Save(account);
        }
    }
}
