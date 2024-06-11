namespace HomeBanking.DTOs
{
    public class LoanApplicationDTO
    {
        public LoanApplicationDTO(long loanId, string toAccountNumber, double amount, string payments)
        {
            LoanId = loanId;
            ToAccountNumber = toAccountNumber;
            Amount = amount;
            Payments = payments;
        }

        public long LoanId { get; set; }
        public string ToAccountNumber { get; set; }
        public double Amount { get; set; }
        public string Payments { get; set; }
    }
}
