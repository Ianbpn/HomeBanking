﻿using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBanking.Repositories.Implementations
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository //Extiende de RepositoryBase para acceder a sus metodos e implementa la interfaz IClientRepository para asegugar que funciones debe cumplir
    {
        public ClientRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Client FindById(long id)
        {
            return FindByCondition(client=>client.Id == id)
                .Include(client=>client.Accounts)
                .Include(client=>client.ClientLoans)
                    .ThenInclude(client=>client.Loan)
                .Include(client => client.Cards)
                .FirstOrDefault();
        }
        public Client FindByEmail(string email)
        {
            return FindByCondition(client => client.Email.ToUpper() == email.ToUpper())
                .Include(client => client.Accounts)
                .Include(client => client.ClientLoans)
                    .ThenInclude(cl => cl.Loan)
                .Include(client => client.Cards)
                .FirstOrDefault();
        }

        public IEnumerable<Client> GetAllClients()
        {
            return FindAll()
                .Include(client => client.Accounts)
                .Include(client => client.ClientLoans)
                    .ThenInclude(client=>client.Loan)
                .Include(client => client.Cards)
                .ToList();
        }

        public void Save(Client client)
        {
            Create(client);
            SaveChanges();
        }
    }
}
