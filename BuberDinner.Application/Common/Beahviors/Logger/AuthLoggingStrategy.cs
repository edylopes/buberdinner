



using BuberDinner.Application.Authentication.Commands.Email;
using BuberDinner.Application.Authentication.Commands.Login;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace BuberDinner.Application.Common.Beahviors.Logger;

public class AuthLoggingStrategy<TRequest> :
ILoggingStrategy<TRequest, OneOf<AuthenticationResult, AppError>>
{
    private readonly ILogger<AuthLoggingStrategy<TRequest>> _logger;
    public AuthLoggingStrategy(ILogger<AuthLoggingStrategy<TRequest>> logger)
    {
        _logger = logger;
    }
    public ILogger Logger => throw new NotImplementedException();

    public Task LogAsync(TRequest request, OneOf<AuthenticationResult, AppError> response)
    {
        switch (request)
        {
            case RegisterCommand cmd:
                LogResponse(response, cmd.Email, "User registration");
                break;
            case LoginCommand cmd:
                LogResponse(response, cmd.Email, "User login");
                break;
            case ConfirmEmailCommand cmd:
                LogResponse(response, cmd.email!, "Email confirmation");
                break;
        }
        return Task.CompletedTask;
    }

    private void LogResponse(IOneOf response, string email, string operation)
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