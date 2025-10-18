
using BuberDinner.Application.Common.Services;
using BuberDinner.Domain.users.Events;
using MediatR;
using Microsoft.Extensions.Logging;


namespace BuberDinner.Application.Events.Handlers.Users
{
    public class SendWelcomeEmailHandler : INotificationHandler<UserRegisteredDomainEvent>
    {
        private readonly ILogger<SendWelcomeEmailHandler> _logger;

        private readonly IEmailService _emailService;
        public SendWelcomeEmailHandler(ILogger<SendWelcomeEmailHandler> logger, IEmailService emailService)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {

            await _emailService.SendAsync(notification.email, "Bem-vindo ao Buber Dinner!", "WelcomeEmail.cshtml", notification.name, notification.userId);

            _logger.LogInformation("Sending welcome email to {Email}", notification.email);

        }
    }
}