using ClaimCheckDemo;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

var builder = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IClaimCheckStore, FileClaimCheckStore>();
        services.AddHostedService<OrderService>();
    })
    .UseNServiceBus(context =>
    {
        var endpointConfiguration = new EndpointConfiguration("ClaimCheckDemo");
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        return endpointConfiguration;
    });

await builder.RunConsoleAsync();
