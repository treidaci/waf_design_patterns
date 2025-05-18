namespace Async_Request_Reply.Services;

public class WorkerService(IBackgroundTaskQueue taskQueue) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await taskQueue.DequeueAsync(stoppingToken);
            await workItem(stoppingToken);
        }
    }
}