using System.IO;
using System.Threading.Tasks;

namespace ClaimCheckDemo;

public class FileClaimCheckStore : IClaimCheckStore
{
    private readonly string _directory = "claim-check-storage";

    public FileClaimCheckStore()
    {
        Directory.CreateDirectory(_directory);
    }

    public async Task<string> StoreAsync(string key, byte[] data)
    {
        var filePath = Path.Combine(_directory, $"{key}.bin");
        await File.WriteAllBytesAsync(filePath, data);
        return filePath;
    }

    public async Task<byte[]> RetrieveAsync(string reference)
    {
        return await File.ReadAllBytesAsync(reference);
    }
}