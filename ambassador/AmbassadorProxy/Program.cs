using OpenTelemetry.Metrics;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add YARP Reverse Proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


// Add Polly for resiliency
builder.Services.AddHttpClient("external-cluster")
    .AddPolicyHandler(GetRetryPolicy());

// Add OpenTelemetry metrics
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics => 
    {
        metrics.AddAspNetCoreInstrumentation()  // Track ASP.NET Core metrics
            .AddHttpClientInstrumentation()  // Track outgoing HTTP calls
            .AddMeter("Yarp.ReverseProxy");  // YARP-specific metrics
        
        metrics.AddConsoleExporter();          // For console output
    });

var app = builder.Build();

// Map reverse proxy routes
app.MapReverseProxy();

// Start the proxy
app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));