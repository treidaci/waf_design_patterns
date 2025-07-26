using System.Threading.Channels;
using CompetingConsumersDemo;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(Channel.CreateBounded<string>(new BoundedChannelOptions(50)
{
    FullMode = BoundedChannelFullMode.Wait
}));
builder.Services.AddHostedService<SimulationService>();

var app = builder.Build();
await app.RunAsync();