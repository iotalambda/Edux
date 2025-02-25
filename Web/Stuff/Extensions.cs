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
}
