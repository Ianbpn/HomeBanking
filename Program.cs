using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add context to the container.
//Se aplica la inyecci�n de dependencia para que los controladores tengan acceso al servicio y puedan utilizar sus metodos
builder.Services.AddDbContext<HomeBankingContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbConnection"))
    );

//Add context to the container
//Se aplica la inyecci�n de dependencia para que los controladores tengan acceso al servicio y puedan utilizar sus metodos
builder.Services.AddScoped<IClientRepository,ClientRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<ICardRepository,CardRepository>();

var app = builder.Build();

// create a scope to get the context and initialize the database
// el scope es el alcance que se le otorga a la aplicaci�n para utilizar la base de datos y que se ejecutara al iniciar la aplicaci�n
using(var scope = app.Services.CreateScope())
{
    try
    {
        var service = scope.ServiceProvider;
        var context = service.GetRequiredService<HomeBankingContext>();
        DBInitializer.Initialize(context); // Otorga los datos iniciales en la base de datos
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ha ocurrido un error al enviar la informaci�n a la base de datos!");
    }
    
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
