namespace BulkheadWorker.Workers;
public class BulkheadPollyWorker(IHttpClientFactory factory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = factory.CreateClient("ClientC");

        while (!stoppingToken.IsCancellationRequested)
        {
            _ = CallService(client);
            await Task.Delay(700, stoppingToken);
        }
    }

    private async Task CallService(HttpClient client)
    {
        var baseUrl = Environment.GetEnvironmentVariable("ClientBaseUrl") ?? "http://localhost:6000";
        try
        {
            Console.WriteLine("[Polly] calling service...");
            var response = await client.GetAsync($"{baseUrl}/status/200");
            Console.WriteLine($"[Polly] response: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Polly] error: {ex.Message}");
        }
    }
}
