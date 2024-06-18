using HomeBanking.DTOs;
using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface ITransactionsService
    {
        void AccountToAccountTransaction(long clientId, NewTransactionDTO newTransactionDTO);
        bool VerifyDataFromPost(NewTransactionDTO transactionDTO);
        bool DoesAccountExist(string accountNumber);
        void AddTransaction(Transaction transaction);
        List<TransactionDTO> GetAllTransactions();
        TransactionDTO GetTransactionById(long id);
    }
}
