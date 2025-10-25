using BuberDinner.Application.Common.Interfaces.Services;
using BuberDinner.Domain.Entities.Users.Events;
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
            await _emailService.SendAsync(notification.Email, WelcomeEmailSubject, WelcomeEmailTemplate, notification.Name, notification.UserId);
            _logger.LogInformation("Sending welcome email to {Email}", notification.Email);
        }
    }
}