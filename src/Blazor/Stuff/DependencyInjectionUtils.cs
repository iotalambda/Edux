using Microsoft.AspNetCore.Components.Web;

namespace Edux.Blazor.Stuff;

public static class DependencyInjectionUtils
{
    public static IServiceCollection AddServicesByScanning(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssemblies(typeof(DependencyInjectionUtils).Assembly)
                .AddClasses(classes => classes.AssignableToAny(typeof(ITransient)))
                .AsSelf()
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );
        services.Scan(scan =>
            scan.FromAssemblies(typeof(DependencyInjectionUtils).Assembly)
                .AddClasses(classes => classes.AssignableToAny(typeof(IScoped)))
                .AsSelf()
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
        services.Scan(scan =>
            scan.FromAssemblies(typeof(DependencyInjectionUtils).Assembly)
                .AddClasses(classes => classes.AssignableToAny(typeof(ISingleton)))
                .AsSelf()
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
        );
        return services;
    }
}
