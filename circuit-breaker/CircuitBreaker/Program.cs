using Polly;
using Polly.CircuitBreaker;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("MyCustomClient", client =>
    {
        client.BaseAddress = new Uri("https://mock.httpstatus.io");
    })
    .AddResilienceHandler("CustomPipeline", (pipeline, context) =>
    {
        // Circuit Breaker
        var circuitBreakerOptions = new CircuitBreakerStrategyOptions<HttpResponseMessage>
        {
            FailureRatio = 0.5, // Break if 50% of requests fail
            MinimumThroughput = 10, // Minimum 10 requests before evaluating failure ratio
            SamplingDuration = TimeSpan.FromSeconds(30), // Time window for evaluating failures
            BreakDuration = TimeSpan.FromSeconds(15), // Duration to keep the circuit open
            ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                .HandleResult(response => !response.IsSuccessStatusCode), // Consider non-success status codes as failures
            OnOpened = args =>
            {
                Console.WriteLine("Circuit opened due to failures.");
                return ValueTask.CompletedTask;
            },
            OnClosed = args =>
            {
                Console.WriteLine("Circuit closed. Operations have resumed.");
                return ValueTask.CompletedTask;
            },
            OnHalfOpened = args =>
            {
                Console.WriteLine("Circuit is half-open. Testing the waters.");
                return ValueTask.CompletedTask;
            }
        };
        
        pipeline.AddCircuitBreaker(circuitBreakerOptions);
    });

var app = builder.Build();

var appStartTime = DateTime.UtcNow;

app.MapGet("/external-data", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("MyCustomClient");

    // Elapsed time since app start
    var elapsed = DateTime.UtcNow - appStartTime;

    // Get total minutes as double
    var minutes = elapsed.TotalMinutes;

    // Use modulo logic to switch every 2 minutes
    // First 2 minutes: /503, next 2 minutes: /200, repeat
    string path = (Math.Floor(minutes / 2) % 2 == 0) ? "/503" : "/200";

    var response = await client.GetAsync(path);
    var content = await response.Content.ReadAsStringAsync();
    return Results.Content($"Requested: {path}\n\n{content}", "text/plain");
});



app.Run();