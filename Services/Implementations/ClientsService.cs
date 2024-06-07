using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
namespace HomeBanking.Services.Implementations
{
    public class ClientsService : iClientsService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        public ClientsService(IClientRepository clientRepository, IAccountRepository accountrepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountrepository;
        }
        public Client ReturnCurrentClient(string userEmail)
        {
            Client currentClient = _clientRepository.FindByEmail(userEmail);
            return currentClient;
        }
    }
}
