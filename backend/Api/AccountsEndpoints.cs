using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Api;

public class AccountsEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/accounts");

        group.MapGet("/", GetAccounts);
        group.MapGet("/{id:int}", GetAccount);
    }

    private static async Task<IResult> GetAccounts(AppDbContext context)
    {
        var result = await context.Accounts.ToListAsync();
        return Results.Json(result);
    }
    
    private static async Task<IResult> GetAccount(int id, AppDbContext context)
    {
        var account = await context.Accounts.FindAsync(id);

        return account is null
            ? Results.NotFound()
            : Results.Ok(account);
    }
}