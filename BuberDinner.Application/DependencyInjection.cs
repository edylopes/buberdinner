using BuberDinner.Application.Services.Authentication;

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BuberDinner.Application.Common.Beahviors;
using BuberDinner.Application.Common.Extensions;

namespace BuberDinner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        // 1. FluentValidation: registra todos os IValidator<T>
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // 2. MediatR 
        services.AddMediatR(typeof(DependencyInjection).Assembly);

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        // 3s. Behaviors MediatR (a ordem importa!)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddLoggingStrategies(Assembly.GetExecutingAssembly());


        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }


}
