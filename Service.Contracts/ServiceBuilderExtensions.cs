using Microsoft.Extensions.DependencyInjection;
using NATS.Client.Hosting;
using Service.Contracts.Communication;

namespace Service.Contracts;

public static class ServiceBuilderExtensions
{
    public static IServiceCollection AddCommunication(this IServiceCollection services)
    {
        services.AddTtmNats();
        return services;
    }
    
    public static IServiceCollection AddTtmHostedServices(this IServiceCollection services)
    {
        // Order is important here as services are started sequentially
        services.AddHostedService<NatsDispatcherWorker>();
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining<Program>()
        });
        return services;
    }
    
    private static void AddTtmNats(this IServiceCollection services)
        => services.AddNats(
            poolSize: 3,
            configureOpts: opts => opts with { SerializerRegistry = NatsJsonCustomSerializerRegistry.Default });
}