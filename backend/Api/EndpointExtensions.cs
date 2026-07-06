using System.Reflection;

namespace backend.Api;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpointType = typeof(IEndpoint);

        var endpoints = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => endpointType.IsAssignableFrom(t)
                        && !t.IsInterface
                        && !t.IsAbstract);

        foreach (var type in endpoints)
        {
            var endpoint = (IEndpoint)Activator.CreateInstance(type)!;
            endpoint.MapEndpoints(app);
        }

        return app;
    }
}