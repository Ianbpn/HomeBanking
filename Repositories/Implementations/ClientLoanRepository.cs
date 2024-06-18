using HomeBanking.Models;

namespace HomeBanking.Repositories.Implementations
{
    public class ClientLoanRepository : RepositoryBase<ClientLoan>,IClientLoanRepository
    {
        public ClientLoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {}
        public void save(ClientLoan clientLoan)
        {
            Create(clientLoan);
            SaveChanges();
        }
    }
}
