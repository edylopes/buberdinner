using BuberDinner.Application.Services.Authentication;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BuberDinner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        
        
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        return services;
    }
}
