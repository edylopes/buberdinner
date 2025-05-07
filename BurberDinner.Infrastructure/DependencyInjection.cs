using BurberDinner.Application.Common.Interfaces.Authentication;
using BurberDinner.Application.Common.Interfaces.Persistence;
using BurberDinner.Application.Common.Interfaces.Services;
using BurberDinner.Domain.Entities;
using BurberDinner.Infrastructure.Authentication;
using BurberDinner.Infrastructure.Configuration;
using BurberDinner.Infrastructure.Persistence;
using BurberDinner.Infrastructure.Services;
//MS
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BurberDinner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.Configure<ConfigurationOptions>(configuration.GetSection(ConfigurationOptions.JWT));
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}