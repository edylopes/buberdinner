using BuberDinner.Application.Authentication.Commands.Login;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace BuberDinner.Application.Common.Beahviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IOneOf
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(
      TRequest request,
      RequestHandlerDelegate<TResponse> next,           
      CancellationToken cancellationToken)
    {
        var response = await next(); //Chama o próximo da cadeia (ou o handler)

        return request switch
        {
            RegisterCommand rc => LogAuthResponse(response, rc.Email, operation: "User registration"),
            LoginCommand lq => LogAuthResponse(response, lq.Email, "User login"),
            _ => response
        };
    }
    private TResponse LogAuthResponse(TResponse response, string email, string operation)
    {

        if (response is OneOf<AuthenticationResult, AppError> oneOfResponse)
        {
            oneOfResponse.Switch(
                success => _logger.LogInformation("✅ {Operation} succeeded for {Email}", operation, email),
                error => _logger.LogWarning("❌ {Operation} failed for email: {Email}: {Error}", operation, email, error.Message)
            );
        }
        return response;

    }

}
