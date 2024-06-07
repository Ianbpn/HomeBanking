using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface iClientsService
    {
        Client ReturnCurrentClient(string userEmail);
    }
}
