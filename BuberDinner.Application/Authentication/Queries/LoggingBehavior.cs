

using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;



namespace BuberDinner.Application.Authentication.Commands.Login
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
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

            var response = await next(); // Chama o próximo da cadeia (ou o handler)

            switch (request)
            {
                case RegisterCommand registerCommand:
                    LogAuthResponse(response, registerCommand.Email, "User registration");
                    break;

                case LoginQuery loginQuery:
                    LogAuthResponse(response, loginQuery.email, " User login");
                    break;
            }

            return response;
        }


        private void LogAuthResponse(TResponse response, string email, string operacao)
        {
            // TResponse é OneOf<AuthenticationResult, AppError>
            if (response is OneOf<AuthenticationResult, AppError> oneOf)
            {
                oneOf.Switch(
                    auth =>
                        _logger.LogInformation("✅ {Operation} for {Email} was successful.", operacao, email),
                    error =>
                        _logger.LogWarning("❌ {Operation} failed for email: {Email}: {Error}", operacao, email, error.message)
                );
            }
            else
            {
                _logger.LogDebug("Unexpected response type for request: {RequestType}", typeof(TRequest).Name);
            }

        }
    }

}