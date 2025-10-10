using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common.Beahviors;
using BuberDinner.Application.Authentication.Commands.Login;
using BuberDinner.Application.Services.Authentication;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BuberDinner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        // 1. Serviços da aplicação

        // 2. FluentValidation: registra todos os IValidator<T>
        services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();

        // 3. MediatR 
        services.AddMediatR(typeof(DependencyInjection).Assembly);

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        // 4. Behaviors MediatR (a ordem importa!)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
}
