using backend.Domain.Entities;

namespace backend.Api;

public class CurrenciesEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/currencies");

        group.MapGet("/", CurrencyHelpers.GetAllCurrencies)
            .WithName("GetAllCurrencies");
        
        group.MapGet("/check", CheckCurrency);
    }

    private static IResult CheckCurrency(string symbol, string isoSymbol, string fullName)
    {
        try
        {
            var testCurrency = new Currency(symbol, isoSymbol, fullName);
            return Results.Text(
                $"Currency found: {testCurrency.FullName}, {testCurrency.IsoSymbol}, {testCurrency.Symbol}");
        }
        catch (Exception e)
        {
            throw new ArgumentException("Couldn't find the specified currency");
        }
    }
}