using System.Threading.Tasks;

namespace ClaimCheckDemo;

public interface IClaimCheckStore
{
    Task<string> StoreAsync(string key, byte[] data);
    Task<byte[]> RetrieveAsync(string reference);
}