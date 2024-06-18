using HomeBanking.DTOs;
using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface IClientsService
    {
        List<ClientDTO> GetAllClients();
        ClientDTO FindClientByEmail(string userEmail);
        ClientDTO GetClientById(long id);
        Client GetFullClientByEmail(string userEmail);
        bool VerifyNewClientData(NewClientDTO newClientDTO);
        bool VerifyIfEmailExists(string email);
        Client NewClient(NewClientDTO newClientDTO);
        void Save(Client client);
    }
}
