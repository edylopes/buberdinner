using BuberDinner.Api.Common.Mapping.Authentication;
using BuberDinner.Application.Common.Mapping;
using Mapster;
using MapsterMapper;

namespace BuberDinner.Api.Common.Mapping;

public static class MappingDependencyInjection
{
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        //Add assembly  of the layers API and Application
        config.Scan(typeof(AuthRegisterMapping).Assembly);
        config.Scan(typeof(AuthResponseMapping).Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}