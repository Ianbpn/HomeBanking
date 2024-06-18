using Microsoft.EntityFrameworkCore;

namespace HomeBanking.Models
{
    public class HomeBankingContext : DbContext
    {
        //HomeBanking extendera la clase DbContext, la cual tiene todo lo requerido para manipulación de la Base de Datos asociada
        public HomeBankingContext(DbContextOptions<HomeBankingContext> options) : base(options) { }

        //Se utiliza el DbSet para que se creen las tablas en la Base de Datos correspondientes al Modelo asignado
        public DbSet<Client> Clients { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<ClientLoan> ClienLoans { get; set; }
        public DbSet<Card> Cards { get; set; }
    }
}