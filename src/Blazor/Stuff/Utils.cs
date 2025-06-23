using System.Collections.Concurrent;
using System.Security.Cryptography;
using Microsoft.JSInterop;

namespace Edux.Blazor.Stuff;

public static class Utils
{
    public static IServiceCollection AddServicesByScanning(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssemblies(typeof(Utils).Assembly)
                .AddClasses(classes => classes.AssignableToAny(typeof(ITransient)))
                .AsSelf()
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );
        services.Scan(scan =>
            scan.FromAssemblies(typeof(Utils).Assembly)
                .AddClasses(classes => classes.AssignableToAny(typeof(IScoped)))
                .AsSelf()
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
        services.Scan(scan =>
            scan.FromAssemblies(typeof(Utils).Assembly)
                .AddClasses(classes => classes.AssignableToAny(typeof(ISingleton)))
                .AsSelf()
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
        );
        return services;
    }

    public static async Task<IJSObjectReference> ImportModule(
        this IJSRuntime js,
        IWebHostEnvironment webHostEnvironment,
        string modulePath,
        CancellationToken ct
    )
    {
        var hash = GetWwwrootFileHash(modulePath, webHostEnvironment);
        var module = await js.InvokeAsync<IJSObjectReference>(
            "import",
            ct,
            [$"{modulePath}?hash={hash}"]
        );
        return module;
    }

    static readonly ConcurrentDictionary<string, string> wwwrootFileHashes = [];

    public static string GetWwwrootFileHash(string filePath, IWebHostEnvironment webHostEnvironment)
    {
        var hash = wwwrootFileHashes.GetOrAdd(
            filePath,
            filePath =>
            {
                var fullPath = Path.Combine(webHostEnvironment.WebRootPath, filePath);
                using var hashAlgo = SHA256.Create();
                using var fs = File.OpenRead(fullPath);
                var hashBytes = hashAlgo.ComputeHash(fs);
                var hash = Convert.ToHexString(hashBytes);
                return hash;
            }
        );

        return hash;
    }
}
