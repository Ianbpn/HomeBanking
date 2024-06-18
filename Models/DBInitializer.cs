using HomeBanking.Enums;

namespace HomeBanking.Models
{
    public class DBInitializer
    { 
        //DBInitializer se enfoca en la carga de datos iniciales para la Base de Datos
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

            if (!context.Accounts.Any())
            {
                Client ianClient = context.Clients.FirstOrDefault(cl => cl.Email == "ianb.782@live.com");
                if (ianClient != null)
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
            if (!context.Loans.Any())
            {
                // Primero creo diferentes prestamos para usar
                var Loan = new Loan[]
                {
                    new Loan {Name= "Hipotecario", MaxAmount=1000000, Paymnets="12,24,36,48,60"},
                    new Loan {Name= "Personal", MaxAmount= 350000, Paymnets="3,6,12,24"},
                    new Loan {Name= "Automotriz", MaxAmount= 600000, Paymnets="12,24,36,48"},
                    new Loan {Name= "Amigable", MaxAmount= 200000, Paymnets="3,6,12,24,36,48,60"}
                };
                context.Loans.AddRange(Loan);
                context.SaveChanges();

                //Ahora voy a darle varios prestamos a mi primer usuario

                var client1 = context.Clients.FirstOrDefault(client => client.Email == "ianb.782@live.com");
                if (client1 != null)
                {
                    //voy a generar manualmente con todos los tipo de prestamo
                    var loan1 = context.Loans.FirstOrDefault(loan => loan.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 750000,
                            ClientId = client1.Id,
                            LoanId = loan1.Id,
                            Payments = "60"
                        };
                        context.ClienLoans.Add(clientLoan1);
                    }

                    var loan2 = context.Loans.FirstOrDefault(loan => loan.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 300000,
                            ClientId = client1.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClienLoans.Add(clientLoan2);
                    }

                    var loan3 = context.Loans.FirstOrDefault(loan => loan.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 100000,
                            ClientId = client1.Id,
                            LoanId = loan3.Id,
                            Payments = "24"
                        };
                        context.ClienLoans.Add(clientLoan3);
                    }

                    var loan4 = context.Loans.FirstOrDefault(loan => loan.Name == "Amigable");
                    if (loan4 != null)
                    {
                        var clientLoan4 = new ClientLoan
                        {
                            Amount = 50000,
                            ClientId = client1.Id,
                            LoanId = loan4.Id,
                            Payments = "3"
                        };
                        context.ClienLoans.Add(clientLoan4);
                    }
                    context.SaveChanges();
                }
            }
            if (!context.Cards.Any())
            {
                var client1 = context.Clients.FirstOrDefault(client => client.Email == "ianb.782@live.com");
                if (client1 != null)
                {
                    //Le agrego 3 tipos de tarjetas, 2 de DEBITO de colores GOLD y Silver, y una de CREDITO de color TITANIUM
                    var cards = new Card[]
                    {
                        new Card
                        {
                            ClientId=client1.Id,
                            CardHolder =client1.FirstName + " " + client1.LastName,
                            Type = CardType.DEBIT.ToString(),
                            Color = CardColor.GOLD.ToString(),
                            Number = "3325-6745-7876-4445",
                            Cvv = 990,
                            FromDate = DateTime.Now,
                            ThruDate = DateTime.Now.AddYears(4),
                        },
                        new Card
                        {
                            ClientId=client1.Id,
                            CardHolder =client1.FirstName + " " + client1.LastName,
                            Type = CardType.CREDIT.ToString(),
                            Color = CardColor.TITANIUM.ToString(),
                            Number = "2234-6745-552-7888",
                            Cvv = 750,
                            FromDate = DateTime.Now,
                            ThruDate = DateTime.Now.AddYears(4),
                        },
                        new Card
                        {
                            ClientId=client1.Id,
                            CardHolder =client1.FirstName + " " + client1.LastName,
                            Type = CardType.DEBIT.ToString(),
                            Color = CardColor.SILVER.ToString(),
                            Number = "2525-7373-6699-2024",
                            Cvv = 777,
                            FromDate = DateTime.Now,
                            ThruDate = DateTime.Now.AddYears(3),
                        }
                    };
                    context.Cards.AddRange(cards);
                    context.SaveChanges();
                }
            }
        }
    }
}
