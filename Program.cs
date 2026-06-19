using DoughBro.src;
using DoughBro.src.Repositories;
using DoughBro.src.Repositories.Interfaces;
using DoughBro.src.Services.Interfaces;
using DoughBro.src.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
//Consider using service extensions
builder.Services.AddSingleton<IDbProvider, DbProvider>();

//Repositories
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

//Services
builder.Services.AddScoped<ITransactionService, TransactionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
