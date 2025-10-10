


using BuberDinner.Application.Common.Services;

using BuberDinner.Domain.users.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuberDinner.Application.Events.Handlers.Users
{
    public class SendWelcomeEmailHandler : INotificationHandler<UserRegisteredDomainEvent>
    {
        // private readonly IEmailService _emailService; //TODO  do this implementarion
        private readonly ILogger<SendWelcomeEmailHandler> _logger;
        public SendWelcomeEmailHandler(ILogger<SendWelcomeEmailHandler> logger)
        {
            //  _emailService = emailService;
            _logger = logger;
        }

        public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending welcome email to {Email}", notification.email);
            /* 
                        await _emailService.SendAsync(
                            to: notification.email,
                            subject: "Welcome to BuberDinner 🎉",
                            body: $"Hello There! Your account was successfully created at {DateTime.UtcNow}."
                        ); */
            await Task.CompletedTask;
        }
    }
}