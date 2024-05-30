using HomeBanking.Database.Models;
using HomeBanking.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add context to the container.
builder.Services.AddDbContext<HomeBankingContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbConnection"))
    );

//Add context to the container
builder.Services.AddScoped<IClientRepository,ClientRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

var app = builder.Build();

// create a scope to get the context and initialize the database

using(var scope = app.Services.CreateScope())
{
    try
    {
        var service = scope.ServiceProvider;
        var context = service.GetRequiredService<HomeBankingContext>();
        DBInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ha ocurrido un error al enviar la información a la base de datos!");
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
