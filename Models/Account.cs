namespace HomeBanking.Models
{
    public class Account
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
        public Client Client { get; set; }
        //Se utiliza la conveción 4 de la relación 1:N, la particularidad de la misma es que Account conoce a quien forma se relaciona, siendo Client y tiene su Id para la navegación en ambos sentidos
        public long ClientId { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}