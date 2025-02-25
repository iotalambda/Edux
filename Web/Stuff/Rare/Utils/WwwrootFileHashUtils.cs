using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace Edux.Web.Stuff.Rare.Utils;

public static class WwwrootFileHashUtils
{
    static readonly ConcurrentDictionary<string, string> wwwrootFileHashes = [];
    public static string GetWwwrootFileHash(string filePath, IWebHostEnvironment webHostEnvironment)
    {
        var hash = wwwrootFileHashes.GetOrAdd(filePath, filePath =>
        {
            var fullPath = Path.Combine(webHostEnvironment.WebRootPath, filePath);
            using var hashAlgo = SHA256.Create();
            using var fs = File.OpenRead(fullPath);
            var hashBytes = hashAlgo.ComputeHash(fs);
            var hash = BitConverter.ToString(hashBytes).Replace("-", "");
            return hash;
        });

        return hash;
    }
}
