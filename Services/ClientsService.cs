using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;

namespace HomeBanking.Services
{
    public class ClientsService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        public ClientsService(IClientRepository clientRepository, IAccountRepository accountrepository) {
            _clientRepository = clientRepository;
            _accountRepository = accountrepository;
        }
    }
}
