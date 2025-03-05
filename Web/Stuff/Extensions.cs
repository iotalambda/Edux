using Azure.Core.Pipeline;
using Azure.Security.KeyVault.Secrets;
using Edux.Web.Stuff.Rare;
using Microsoft.JSInterop;
using System.Reflection;

namespace Edux.Web.Stuff;

public static class Extensions
{
    public static IServiceCollection AddServicesFromAssemblies(this IServiceCollection services, Assembly[] assemblies)
    {
        assemblies = assemblies.Distinct().ToArray();

        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableToAny(typeof(IScoped)))
            .AsSelf()
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableToAny(typeof(ISingleton)))
            .AsSelf()
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableToAny(typeof(ITransient)))
            .AsSelf()
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }

    public static async Task<IJSObjectReference> ImportModule(this IJSRuntime js, IWebHostEnvironment webHostEnvironment, string modulePath, CancellationToken ct)
    {
        var hash = Rare.Utils.WwwrootFileHashUtils.GetWwwrootFileHash(modulePath, webHostEnvironment);
        var module = await js.InvokeAsync<IJSObjectReference>("import", ct, [$"{modulePath}?hash={hash}"]);
        return module;
    }

    public static SecretClientOptions WithCustomDomainSupport(this SecretClientOptions options)
    {
        options.DisableChallengeResourceVerification = true; // Configured custom domain wont end with *.vault.azure.net.
        HttpMessageHandler handler = new HttpClientHandler();
        handler = new RequireCanonicalHostNameDelegatingHandler(h => h.EndsWith(".vault.azure.net")) // Ensure the canonical host name is used for actual Key Vault requests directly.
        {
            InnerHandler = handler
        };
        options.Transport = new HttpClientTransport(handler);
        return options;
    }
}
