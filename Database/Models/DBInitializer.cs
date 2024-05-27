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
        }
    }
}
