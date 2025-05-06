This Ambassador pattern implementation uses [YARP](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/yarp/getting-started?view=aspnetcore-9.0) to implement a reverse proxy that clients can call this way:
```bash
curl http://localhost:5284/objects
```

It uses [Polly](https://www.pollydocs.org/) for resiliency and [OpenTelemetry](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-with-otel) metrics with console output.

A different implementation can use a middleware as the Ambassador:
```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public class AmbassadorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpClient _httpClient;
    private readonly AsyncCircuitBreakerPolicy _circuitBreaker;

    public AmbassadorMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _httpClient = httpClientFactory.CreateClient("RemoteService");
        
        // Configure circuit breaker
        _circuitBreaker = Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromMinutes(1));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Start measuring request latency
        var startTime = DateTime.UtcNow;
        
        try
        {
            // Determine location of remote service
            var targetUrl = DetermineServiceLocation(context.Request);
            
            // Add tracing information
            context.Request.Headers.Add("X-Trace-ID", Guid.NewGuid().ToString());
            
            // Execute request through circuit breaker
            var response = await _circuitBreaker.ExecuteAsync(async () => 
            {
                var request = CreateProxiedRequest(context, targetUrl);
                return await _httpClient.SendAsync(request);
            });
            
            // Return response to client
            await WriteProxyResponseAsync(context, response);
        }
        catch (Exception ex)
        {
            // Handle exceptions
            context.Response.StatusCode = 502;
            await context.Response.WriteAsync($"Ambassador error: {ex.Message}");
        }
        finally
        {
            // Log request latency
            var requestTime = DateTime.UtcNow - startTime;
            Console.WriteLine($"Request took {requestTime.TotalMilliseconds}ms");
        }
    }
    
    // Helper methods would be implemented here...
}

// Program.cs or Startup.cs
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient("RemoteService", client =>
        {
            client.BaseAddress = new Uri("https://remote-service-url/");
            // Configure client (timeout, headers, etc.)
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<AmbassadorMiddleware>();
    }
}

```