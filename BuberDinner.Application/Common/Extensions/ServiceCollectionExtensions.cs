
using System.Reflection;

using BuberDinner.Application.Common.Beahviors.Logger;

using Microsoft.Extensions.DependencyInjection;

namespace BuberDinner.Application.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoggingStrategies(this IServiceCollection services, params Assembly[] assemblies)
    {
        var strategyInterface = typeof(ILoggingStrategy<,>);

        var implementations = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                !t.IsAbstract &&
                !t.IsInterface &&
                t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == strategyInterface))
            .ToList();

        foreach (var implementation in implementations)
        {
            var interfaces = implementation
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == strategyInterface);

            foreach (var iface in interfaces)
            {
                services.AddTransient(iface, implementation);
            }
        }

        return services;
    }
}
