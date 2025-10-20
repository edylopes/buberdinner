
using BuberDinner.Application.Common.Services;
using BuberDinner.Domain.users.Events;
using MediatR;
using Microsoft.Extensions.Logging;


namespace BuberDinner.Application.Events.Handlers.Users
{
    public class SendWelcomeEmailHandler : INotificationHandler<UserRegisteredDomainEvent>
    {
        private readonly ILogger<SendWelcomeEmailHandler> _logger;
        const string WelcomeEmailTemplate = "WelcomeEmail.cshtml";
        const string WelcomeEmailSubject = "Bem-vindo ao Buber Dinner!";
        private readonly IEmailService _emailService;
        public SendWelcomeEmailHandler(ILogger<SendWelcomeEmailHandler> logger, IEmailService emailService)
        {
            _emailService = emailService;
            _logger = logger;
        }
        public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            await _emailService.SendAsync(notification.email, WelcomeEmailSubject, WelcomeEmailTemplate, notification.name, notification.userId);
            _logger.LogInformation("Sending welcome email to {Email}", notification.email);
        }
    }
}