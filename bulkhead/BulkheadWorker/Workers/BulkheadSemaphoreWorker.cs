namespace BulkheadWorker.Workers;

public class BulkheadSemaphoreWorker : BackgroundService
{
    private readonly SemaphoreSlim _poolA = new SemaphoreSlim(3);
    private readonly SemaphoreSlim _poolB = new SemaphoreSlim(2);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _ = RunInPool(_poolA, "Task A", 1000);
            _ = RunInPool(_poolB, "Task B", 1500);
            await Task.Delay(500, stoppingToken);
        }
    }

    private async Task RunInPool(SemaphoreSlim semaphore, string label, int delay)
    {
        await semaphore.WaitAsync();
        try
        {
            Console.WriteLine($"[{label}] started at {DateTime.UtcNow:HH:mm:ss}");
            await Task.Delay(delay);
            Console.WriteLine($"[{label}] completed at {DateTime.UtcNow:HH:mm:ss}");
        }
        finally
        {
            semaphore.Release();
        }
    }
}