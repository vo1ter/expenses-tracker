using backend.Domain.Entities;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); 
var context = new AppDbContext();

builder.Services.AddOpenApi();

var app = builder.Build();

Console.WriteLine("Checking if the env is dev...");

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Env is dev, registering endpoint for openapi documentation");
    app.MapOpenApi();
}

Console.WriteLine("Checking DB connection...");
try
{
    context.Database.CanConnect();
    Console.WriteLine("Can connect to the DB!");
}
catch (Exception e)
{
    throw new ApplicationException($"Can not connect to the database: {e.Message}");
}

Console.WriteLine("Checking if the table and tables are present...");

try
{
    context.Database.EnsureCreated();
    Console.WriteLine("The schema and tables are present!");
}
catch (Exception e)
{
    throw new ApplicationException(
        $"Could not check or create the schema and tables: {e}",
        e
    );
}


app.UseHttpsRedirection();

app.MapGet("/currencies", CurrencyHelpers.GetAllCurrencies)
    .WithName("GetAllCurrencies");


app.MapGet("/currencies/check", (char symbol, string isoSymbol, string fullName) =>
    {
        try
        {
            var testCurrency = new Currency(symbol, isoSymbol, fullName);
            return Results.Text($"Currency found: {testCurrency.FullName}, {testCurrency.IsoSymbol}, {testCurrency.Symbol}");
        }
        catch (Exception e)
        {
            return Results.Text($"Exception thrown: {e.Message}");
        }
    })
    .WithName("CheckCurrency");

app.MapGet("/accounts", async (Guid? id) =>
    {
        try
        {
            if (id != null)
            {
                var result = await context.Accounts.FindAsync(id);
                return Results.Json(result);
            }
            else
            {
                var result = await context.Accounts.ToListAsync();
                return Results.Json(result);
            }
        }
        catch (Exception e)
        {
            return Results.Text($"Exception thrown: {e.Message}");
        }
    })
    .WithName("GetAccounts");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}