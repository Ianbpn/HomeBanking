using HomeBanking.Database.Models;

namespace HomeBanking.Repositories.Implementations
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAllClients();
        void Save(Client client);
        Client FindById(long id);
    }
}
