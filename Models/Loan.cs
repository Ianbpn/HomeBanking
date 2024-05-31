namespace HomeBanking.Models
{
    public class Loan
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double MaxAmount { get; set; }
        public string Paymnets { get; set; }
        public ICollection<ClientLoan> ClienLoans { get; set; }
    }
}