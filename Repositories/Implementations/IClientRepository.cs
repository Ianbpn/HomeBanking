﻿using HomeBanking.Models;

namespace HomeBanking.Repositories.Implementations
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAllClients();
        void Save(Client client);
        Client FindById(long id);
        Client FindByEmail(string email);
    }
}
