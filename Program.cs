using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using HomeBanking.Services;
using HomeBanking.Services.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add context to the container.
//Se aplica la inyección de dependencia para que los controladores tengan acceso al servicio y puedan utilizar sus metodos
builder.Services.AddDbContext<HomeBankingContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbConnection"))
    );

//Add context to the container
//Se aplica la inyección de dependencia para que los controladores tengan acceso al servicio y puedan utilizar sus metodos
//Inyeccion de Repositorios
builder.Services.AddScoped<IClientRepository,ClientRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<ICardRepository,CardRepository>();
builder.Services.AddScoped<IClientLoanRepository, ClientLoanRepository>();

//Inyeccion de Servicios
builder.Services.AddScoped<IAccountsService, AccountsService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IClientsService, ClientsService>();
builder.Services.AddScoped<ITransactionsService, TransactionService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IClientLoanService,ClientLoanService>();
builder.Services.AddScoped<IAuthService, AuthService>();

//Implementación del sistema de autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        options.LoginPath = new PathString("/index.html");
    });

//Implementación del sistema de autorización
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ClientOnly", policy => policy.RequireClaim("Client"));
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin"));
});

var app = builder.Build();

// create a scope to get the context and initialize the database
// el scope es el alcance que se le otorga a la aplicación para utilizar la base de datos y que se ejecutara al iniciar la aplicación
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
        logger.LogError(ex, "Ha ocurrido un error al enviar la información a la base de datos!");
    }
    
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
