using Microsoft.Extensions.Caching.Memory;

namespace CacheAsideWorker;

public class CacheAsideWorkerService(IMemoryCache cache, IDataStore dataStore) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            const int productId = 1; // for demo purposes
            var product = await GetProductAsync(productId);
            Console.WriteLine($"Fetched product: {product.Name} at {DateTime.UtcNow:HH:mm:ss}");
            await Task.Delay(5000, stoppingToken);
        }
    }

    private async Task<Product> GetProductAsync(int productId)
    {
        var cacheKey = $"product:{productId}";

        if (cache.TryGetValue<Product>(cacheKey, out var cachedProduct))
        {
            Console.WriteLine("[Cache] Hit");
            return cachedProduct!;
        }

        Console.WriteLine("[Cache] Miss. Fetching from data store...");
        var product = await dataStore.FetchProductByIdAsync(productId);

        if (product is not null)
        {
            cache.Set(cacheKey, product, TimeSpan.FromMinutes(10));
        }

        return product!;
    }
}