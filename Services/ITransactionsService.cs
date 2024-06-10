using HomeBanking.DTOs;

namespace HomeBanking.Services
{
    public interface ITransactionsService
    {
        void NewTransaction(long clientId, NewTransactionDTO newTransactionDTO);
        bool VerifyDataFromPost(NewTransactionDTO transactionDTO);
        bool DoesAccountExist(string accountNumber);
    }
}
