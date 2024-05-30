using HomeBanking.Database.Models;

namespace HomeBanking.DTOs
{
    public class TransactionDTO
    {
        public long Id { get; set; }
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TransactionDTO(Transaction transaction)
        {
            this.Id = transaction.Id;
            this.Type = transaction.Type;
            this.Amount = transaction.Amount;
            this.Description = transaction.Description;
            this.Date = transaction.Date;
        }
    }
}