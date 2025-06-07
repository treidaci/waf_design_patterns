using Polly;
using BulkheadWorker.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient("ClientA")
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        MaxConnectionsPerServer = 10
    });

builder.Services.AddHttpClient("ClientB")
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        MaxConnectionsPerServer = 5
    });

builder.Services.AddHttpClient("ClientC")
    .AddPolicyHandler(Policy.BulkheadAsync<HttpResponseMessage>(
        maxParallelization: 3,
        maxQueuingActions: 5
    ));

builder.Services.AddHostedService<BulkheadSemaphoreWorker>();
builder.Services.AddHostedService<BulkheadHttpClientWorker>();
builder.Services.AddHostedService<BulkheadPollyWorker>();

var host = builder.Build();
host.Run();