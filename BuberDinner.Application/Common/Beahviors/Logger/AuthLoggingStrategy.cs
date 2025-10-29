
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces;
using BuberDinner.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace BuberDinner.Application.Common.Beahviors.Logger;

public class AuthLoggingStrategy<TRequest, TResponse> :
ILoggingStrategy<TRequest, TResponse>
{
    private readonly ILogger<AuthLoggingStrategy<TRequest, TResponse>> _logger;
    public AuthLoggingStrategy(ILogger<AuthLoggingStrategy<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public Task LogAsync(TRequest request, TResponse response)
    {
        if (request is ICommand<TResponse> command)
        {

            LogResponse(response, command.Email!, command.Operation);

        }

        return Task.CompletedTask;
    }

    private void LogResponse(TResponse response, string email, string operation)
    {
        if (response is OneOf<AuthenticationResult, AppError> oneOf)
        {
            oneOf.Switch(
                success => _logger.LogInformation("✅ {Operation} succeeded for {Email}", operation, email),
                error => _logger.LogWarning("❌ {Operation} failed for {Email}: {Error}", operation, email, error.Message)
            );
        }
    }

}