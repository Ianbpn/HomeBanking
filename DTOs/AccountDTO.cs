using HomeBanking.Models;

namespace HomeBanking.DTOs
{
    public class AccountDTO
    {
        public long Id { get; set; }
        public string Number { get; set; }

        public DateTime CreationDate { get; set; }

        public double Balance { get; set; }
        public ICollection<TransactionDTO> Transactions { get; set; }
        public AccountDTO(Account account)
        {
            this.Id = account.Id;
            this.Number = account.Number;
            this.CreationDate = account.CreationDate;
            this.Balance = account.Balance;
            this.Transactions = account.Transactions.Select(t => new TransactionDTO(t)).ToList();
        }
    }
}