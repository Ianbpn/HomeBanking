using HomeBanking.Database.Models;

namespace HomeBanking.DTOs
{
    public class ClientLoanDTO
    {
        public ClientLoanDTO(ClientLoan clientLoan)
        {
            Id = clientLoan.Id;
            LoanId = clientLoan.LoanId;
            Name = clientLoan.Loan.Name;
            Amount = clientLoan.Amount;
            Payments = clientLoan.Payments;
        }

        public long Id  { get; set; }
        public long LoanId  { get; set; }
        public double Amount { get; set; }
        public string Name { get; set; }
        public string Payments { get; set; }
    }
}
