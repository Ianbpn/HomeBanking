using HomeBanking.Models;

namespace HomeBanking.DTOs
{
    public class ClientDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<AccountClientDTO> Accounts { get; set; }
        public ICollection<ClientLoanDTO> Loans { get; set; }
        public ICollection<CardDTO> Cards { get; set; }
        public ClientDTO(Client client)
        {
            this.Id = client.Id;
            this.FirstName = client.FirstName;
            this.LastName = client.LastName;
            this.Email = client.Email;
            this.Accounts = client.Accounts.Select(account => new AccountClientDTO(account)).ToList();
            this.Loans = client.ClientLoans.Select(loan=> new ClientLoanDTO(loan)).ToList();
            this.Cards = client.Cards.Select(card=>new CardDTO(card)).ToList();
        }
    }
}
