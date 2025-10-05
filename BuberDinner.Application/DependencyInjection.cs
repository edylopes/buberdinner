using BuberDinner.Application.Authentication.Commands.Login;
using BuberDinner.Application.Authentication.Queries;
using BuberDinner.Application.Services.Authentication;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BuberDinner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {


        // 1. Serviços da aplicação
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        // 2. FluentValidation: registra todos os IValidator<T>
        services.AddValidatorsFromAssemblyContaining<LoginQueryValidator>();

        // 3. MediatR 
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        // 4. Behaviors do MediatR (a ordem importa!)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));




        return services;
    }
}
