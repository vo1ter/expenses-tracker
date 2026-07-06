using System.Data;
using backend.Api;
using backend.Infrastructure.Data;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

Console.WriteLine("Checking if the env is dev...");

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Env is dev, registering endpoint for openapi documentation. Link: http://localhost:5132/swagger");
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    Console.WriteLine("Checking DB connection...");

    try
    {
        await context.Database.CanConnectAsync();
        Console.WriteLine("Can connect to the DB!");
    }
    catch (Exception e)
    {
        throw new ConnectionAbortedException($"Cannot connect to DB: {e.Message}", e);
    }

    Console.WriteLine("Checking schema...");

    try
    {
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("Schema is ready!");
    }
    catch (Exception e)
    {
        throw new DataException(
            $"Schema check failed: {e.Message}",
            e
        );
    }
}

app.UseHttpsRedirection();

app.MapEndpoints();

await app.RunAsync();