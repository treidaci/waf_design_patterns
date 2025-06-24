namespace CacheAsideWorker;

using System.Threading.Tasks;

public interface IDataStore
{
    Task<Product> FetchProductByIdAsync(int productId);
}

public class MockDataStore : IDataStore
{
    public Task<Product> FetchProductByIdAsync(int productId)
    {
        return Task.FromResult(new Product
        {
            Id = productId,
            Name = $"Product {productId}",
            Price = 99.99m
        });
    }
}