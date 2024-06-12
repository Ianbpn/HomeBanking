using HomeBanking.DTOs;
using HomeBanking.Exceptions;
using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace HomeBanking.Services.Implementations
{
    public class ClientsService : IClientsService
    {
        private readonly IClientRepository _clientRepository;
        public ClientsService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public ClientDTO FindClientByEmail(string userEmail)
        {
            var currentClient = _clientRepository.FindByEmail(userEmail);
            var currentClientDTO = new ClientDTO(currentClient);
            return currentClientDTO;
        }

        public List<ClientDTO> GetAllClients()
        {
            var clients = _clientRepository.GetAllClients();
            var clientsDTO = clients.Select(clients => new ClientDTO(clients)).ToList();
            return clientsDTO;
        }
        public ClientDTO GetClientById(long id)
        {
            var client = _clientRepository.FindById(id);
            var clientDTO = new ClientDTO(client);
            return clientDTO;
        }

        public Client GetFullClientByEmail(string userEmail)
        {
            //Devuelve el cliente con sus datos sensibles, usado en para el Login
            var currentClient = _clientRepository.FindByEmail(userEmail);
            return currentClient;
        }
        public bool VerifyNewClientData(NewClientDTO newClientDTO)
        {
            try
            {
                //Verifica la Informacion proveniente del Post
                if (newClientDTO.FirstName.IsNullOrEmpty() || newClientDTO.LastName.IsNullOrEmpty()
                    || newClientDTO.Password.IsNullOrEmpty() || newClientDTO.Email.IsNullOrEmpty())
                {
                    throw new CustomException("Missing Fields", 400);
                }
                return true;

            }
            catch (CustomException ex)
            {

                throw new CustomException(ex.message, ex.statusCode);
            }
        }
        public bool VerifyIfEmailExists(string email)
        {
            Client client = _clientRepository.FindByEmail(email);
            //Si encuentra por mail retorna un true, en caso contrario un false
            if (client == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public Client NewClient(NewClientDTO newClientDTO)
        {
            //Crea un nuevo cliente con los datos provenientes del Post
            Client newClient = new()
            {
                FirstName = newClientDTO.FirstName,
                LastName = newClientDTO.LastName,
                Password = newClientDTO.Password,
                Email = newClientDTO.Email
            };
            //Luego lo guarda
            Save(newClient);
            return newClient;
        }
        public void Save(Client client)
        {
            _clientRepository.Save(client); 
        }
    }
}

