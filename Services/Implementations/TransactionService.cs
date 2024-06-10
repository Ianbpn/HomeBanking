using HomeBanking.DTOs;
using HomeBanking.Exceptions;
using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using Microsoft.IdentityModel.Tokens;
using System.Net;


namespace HomeBanking.Services.Implementations
{
    public class TransactionService : ITransactionsService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountsService _accountsService;

        public TransactionService(ITransactionRepository transactionRepository, IAccountsService accountsService)
        {
            _transactionRepository = transactionRepository;
            _accountsService = accountsService;
        }
        public void NewTransaction(long clientId, NewTransactionDTO newTransactionDTO)
        {
            if (VerifyDataFromPost(newTransactionDTO))
            {
                var accounts = _accountsService.GetAccountsByClient(clientId);
                if (accounts.Any(a => a.Number == newTransactionDTO.FromAccountNumber))
                {
                    var fromAccount = _accountsService.FindAccountByNumber(newTransactionDTO.FromAccountNumber);
                    var toAccount = _accountsService.FindAccountByNumber(newTransactionDTO.ToAccountNumber);
                    if (fromAccount.Balance < newTransactionDTO.Amount)
                    {
                        throw new CustomException("Esta cuenta no tiene los fondos suficientes", HttpStatusCode.BadRequest);
                    }
                    var DebitTransaction = new Transaction
                    {
                        AccountId = fromAccount.Id,
                        Amount = -newTransactionDTO.Amount,
                        Date = DateTime.Now,
                        Description = newTransactionDTO.Description,
                        Type = Enums.TransactionType.DEBIT
                    };
                    var CreditTransaction = new Transaction
                    {
                        AccountId = toAccount.Id,
                        Amount = newTransactionDTO.Amount,
                        Date = DateTime.Now,
                        Description = newTransactionDTO.Description,
                        Type = Enums.TransactionType.CREDIT
                    };
                    fromAccount.Transactions.Add(DebitTransaction);
                    toAccount.Transactions.Add(CreditTransaction);
                    fromAccount.Balance = fromAccount.Balance - newTransactionDTO.Amount;
                    toAccount.Balance = toAccount.Balance + newTransactionDTO.Amount;

                    _transactionRepository.Save(DebitTransaction);
                    _transactionRepository.Save(CreditTransaction);
                    _accountsService.Save(toAccount);
                    _accountsService.Save(fromAccount);
                }
                else
                {
                    throw new CustomException("Esta cuenta no pertenece al usuario", HttpStatusCode.BadRequest);
                }
            }
        }

        public bool VerifyDataFromPost(NewTransactionDTO transactionDTO)
        {
            if (transactionDTO.FromAccountNumber.IsNullOrEmpty() || transactionDTO.ToAccountNumber.IsNullOrEmpty()
                || transactionDTO.Description.IsNullOrEmpty() || transactionDTO.Amount <= 0)
            {
                throw new CustomException("Verificar los campos vacios", HttpStatusCode.BadRequest);
            }
            else if (transactionDTO.ToAccountNumber.Equals(transactionDTO.FromAccountNumber))
            {
                throw new CustomException("Los numeros de cuenta no pueden ser identicos", HttpStatusCode.BadRequest);
            }
            if (!DoesAccountExist(transactionDTO.FromAccountNumber) || !DoesAccountExist(transactionDTO.ToAccountNumber))
            {
                throw new CustomException($"Una de las cuentas no a sido encontrada", HttpStatusCode.BadRequest);
            }
            return true;
        }
        public bool DoesAccountExist(string accountNumber)
        {
            Account account = _accountsService.FindAccountByNumber(accountNumber);
            if (account == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
