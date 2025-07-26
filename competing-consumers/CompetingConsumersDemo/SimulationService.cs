using System.Collections.Concurrent;
using System.Threading.Channels;
using Microsoft.Extensions.Hosting;

namespace CompetingConsumersDemo;

public class SimulationService(Channel<string> queue) : BackgroundService
{
    private readonly Random _random = new();
    private readonly int _producerCount = new Random().Next(2, 4);
    private readonly int _consumerCount = new Random().Next(3, 5);
    private readonly ConcurrentDictionary<string, int> _retryTracker = new();
    private const int MaxRetries = 3;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine($"🌱 Producers: {_producerCount}, Consumers: {_consumerCount}");

        for (var i = 0; i < _producerCount; i++)
        {
            var id = i + 1;
            _ = Task.Run(() => RunProducerAsync(id, stoppingToken), stoppingToken);
        }

        for (var i = 0; i < _consumerCount; i++)
        {
            var id = i + 1;
            _ = Task.Run(() => RunConsumerAsync(id, stoppingToken), stoppingToken);
        }

        await Task.Delay(TimeSpan.FromSeconds(40), stoppingToken);
        Console.WriteLine("🛑 Simulation complete.");
    }

    private async Task RunProducerAsync(int producerId, CancellationToken token)
    {
        var messageCount = 0;
        while (!token.IsCancellationRequested)
        {
            var messageId = $"Msg-{producerId}-{++messageCount}";
            await queue.Writer.WriteAsync(messageId, token);
            Console.WriteLine($"[Producer {producerId}] ➤ Sent: {messageId}");
            await Task.Delay(_random.Next(100, 500), token);
        }
    }

    private async Task RunConsumerAsync(int consumerId, CancellationToken token)
    {
        while (await queue.Reader.WaitToReadAsync(token))
        {
            if (!queue.Reader.TryRead(out var message)) continue;
            try
            {
                await ProcessMessageAsync(consumerId, message, token);
            }
            catch (Exception ex)
            {
                var retryCount = _retryTracker.AddOrUpdate(message, 1, (_, current) => current + 1);
                if (retryCount <= MaxRetries)
                {
                    Console.WriteLine($"[Consumer {consumerId}] 🔁 Retrying {message} ({retryCount}/{MaxRetries})");
                    await queue.Writer.WriteAsync(message, token);
                }
                else
                {
                    Console.WriteLine($"[Consumer {consumerId}] ☠️ Poison message detected: {message} ({ex.Message})");
                }
            }
        }
    }

    private async Task ProcessMessageAsync(int consumerId, string message, CancellationToken token)
    {
        // Simulate failure and poison conditions
        if (message.Contains('3') && _random.NextDouble() < 0.5)
            throw new InvalidOperationException("Simulated processing failure.");

        if (message.Contains("fail"))
            throw new Exception("Malformed/poison message encountered.");

        Console.WriteLine($"[Consumer {consumerId}] ✅ Processed: {message}");
        await Task.Delay(_random.Next(200, 700), token);
    }
}