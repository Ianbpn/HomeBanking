using HomeBanking.Database.Models;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace HomeBanking.DTOs
{
    public class ClientDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<AccountClientDTO> Accounts { get; set; }
        public ClientDTO(Client client)
        {
            this.Id = client.Id;
            this.FirstName = client.FirstName;
            this.LastName = client.LastName;
            this.Email = client.Email;
            this.Accounts = client.Accounts.Select(a => new AccountClientDTO(a)).ToList();
        }
    }
}
