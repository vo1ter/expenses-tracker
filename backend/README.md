# Expenses tracker - backend

## App settings

appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=host; Port=5432; Username=usr; Password=securepassword; Database=database"
  }
}
```

## For developers

### Adding new routes

You can add new routes by creating a class in Api folder. The class have to implement the IEndpoint interface. Example:

```c#
public class MyEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/myendpoint");

        group.MapGet("/", MyFunction);
    }

    private static Task<IResult> MyFunction()
    {
        return Result.Ok();
    }
}
```

You can also connect your class to the database by adding AppDbContext parameter to the method that requires it. Example:
```c#
public class MyEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/myendpoint");

        group.MapGet("/", MyFunction);
    }

    private static async Task<IResult> MyFunction(AppDbContext context)
    {
        await context.Database.CanConnectAsync();
        return Result.Ok();
    }
}
```