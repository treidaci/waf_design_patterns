namespace BulkheadWorker.Workers;
public class BulkheadHttpClientWorker(IHttpClientFactory factory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var clientA = factory.CreateClient("ClientA");
        var clientB = factory.CreateClient("ClientB");

        while (!stoppingToken.IsCancellationRequested)
        {
            _ = CallService(clientA, "ClientA");
            _ = CallService(clientB, "ClientB");
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task CallService(HttpClient client, string label)
    {
        var baseUrl = Environment.GetEnvironmentVariable("ClientBaseUrl") ?? "http://localhost:6000";
        try
        {
            Console.WriteLine($"[{label}] calling service...");
            var response = await client.GetAsync($"{baseUrl}/delay/2");
            Console.WriteLine($"[{label}] response: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{label}] error: {ex.Message}");
        }
    }
}