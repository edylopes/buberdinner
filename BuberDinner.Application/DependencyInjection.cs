using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common.Beahviors;
using BuberDinner.Application.Authentication.Queries.Login;
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
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        // 2. FluentValidation: registra todos os IValidator<T>
        services.AddValidatorsFromAssemblyContaining<LoginQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();


        // 3. MediatR 
        services.AddMediatR(typeof(DependencyInjection).Assembly);

        // 4. Behaviors do MediatR (a ordem importa!)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        return services;
    }
}
