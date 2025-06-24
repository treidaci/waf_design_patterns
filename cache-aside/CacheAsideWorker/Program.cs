using CacheAsideWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IDataStore, MockDataStore>();
builder.Services.AddHostedService<CacheAsideWorkerService>();

var host = builder.Build();
host.Run();