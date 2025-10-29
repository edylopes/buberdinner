using BuberDinner.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BuberDinner.Application.Common.Beahviors;
using BuberDinner.Application.Common.Beahviors.Logger;

namespace BuberDinner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        // 1. FluentValidation: registra todos os IValidator<T>
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        // 2. MediatR 
        services.AddMediatR(typeof(DependencyInjection).Assembly);

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        // 3s. Behaviors MediatR (a ordem importa!)
        //services.AddLoggingStrategies(AppDomain.CurrentDomain.GetAssemblies());

        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddTransient(typeof(ILoggingStrategy<,>), typeof(AuthLoggingStrategy<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }


}
