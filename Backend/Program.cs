using DoughBro.src.Repositories;
using DoughBro.src.Repositories.Interfaces;
using DoughBro.src.Services.Interfaces;
using DoughBro.src.Services;
using DoughBro.src.Extensions;
using DoughBro.src.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
//builder.Services.AddCustomCors(builder.Configuration);
builder.Services.AddFirebaseAuthentication(builder.Configuration);

//Consider using service extensions
builder.Services.AddSingleton<IDbProvider, DbProvider>();
builder.Services.AddHttpClient("PlaidHttpClient", client =>
{
    var env = builder.Configuration["Plaid:Environment"] ?? "sandbox";
    client.BaseAddress = new Uri($"https://{env}.plaid.com");
});
// Repositories
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPlaidService, PlaidService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
//app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
