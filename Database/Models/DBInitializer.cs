namespace HomeBanking.Database.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client {Email= "ianb.782@live.com",FirstName="Ian",
                        LastName="Pereyra",Password="123456"},
                    new Client {Email= "marcelo@gmail.com",FirstName="Marcelo",
                        LastName="Diaz",Password="123456"},
                    new Client {Email= "julieta@gmail.com",FirstName="Julieta",
                        LastName="Gonzalez",Password="123456"},
                    new Client {Email= "nicolas@gmail.com",FirstName="Nicolas",
                        LastName="Blanco",Password="123456"},
                    new Client {Email= "leila@gmail.com",FirstName="Leila",
                        LastName="Quilogran",Password="123456"},
                    new Client {Email= "agustin@gmail.com",FirstName="Agustin",
                        LastName="Ramallo",Password="123456"},
                    new Client {Email= "valentin@gmail.com",FirstName="Valentin",
                        LastName="De Igartua",Password="123456"}
                };
                context.Clients.AddRange(clients);
                //Guardar los cambios en la base de datos
                context.SaveChanges();
            }

            if (!context.Accounts.Any()) {
                Client ianClient = context.Clients.FirstOrDefault(cl => cl.Email == "ianb.782@live.com");
                if(ianClient != null)
                {
                    var ianAccounts = new Account[]
                    {
                        new Account {Number="IAN001", CreationDate=DateTime.Now, Balance=15000, ClientId=ianClient.Id},
                        new Account {Number="IAN002", CreationDate=DateTime.Now, Balance=25000, ClientId=ianClient.Id}
                    };

                    context.Accounts.AddRange(ianAccounts);
                    context.SaveChanges();
                }
            }
            if (!context.Transactions.Any())
            {
                Account ianAccount1 = context.Accounts.FirstOrDefault(a => a.Number == "IAN001");
                if (ianAccount1 != null)
                {
                    var transaction = new Transaction[]
                    {
                        new Transaction {AccountId= ianAccount1.Id,Amount= 25000, Date=DateTime.Now.AddDays(-1),
                            Description="La Transaccion fue recibida", Type = TransactionType.CREDIT},
                        new Transaction {AccountId= ianAccount1.Id,Amount= -7000, Date=DateTime.Now.AddHours(-5),
                            Description="La Compra a en Steam fue realizada con exito", Type = TransactionType.DEBIT},
                        new Transaction {AccountId= ianAccount1.Id,Amount= -4000, Date=DateTime.Now.AddMinutes(-15),
                            Description="La transferencia a tu segunda cuenta fue exitosa", Type = TransactionType.DEBIT}
                    };
                    context.Transactions.AddRange(transaction);
                    context.SaveChanges();
                }
            }
        }
    }
}
