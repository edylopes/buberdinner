using BuberDinner.Application.Common.Interfaces;
using BuberDinner.Contracts.Authentication.Email;
using BuberDinner.Domain.Entities.Users.Events;


namespace BuberDinner.Application.Events.Handlers.Users;

public class SendWelcomeEmailHandler : INotificationHandler<UserRegisteredDomainEvent>
{
    private const string WelcomeEmailTemplate = "WelcomeEmail.cshtml";
    private const string WelcomeEmailSubject = "Bem-vindo ao Buber Dinner!";
    private readonly IEmailQueue _emailQueue;

    public SendWelcomeEmailHandler(IEmailQueue emailQueue)
    {
        _emailQueue = emailQueue;
    }

    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {

        var message = new EmailMessage(notification.Email, WelcomeEmailSubject, WelcomeEmailTemplate, notification.Name,
            notification.UserId);

        await _emailQueue.EnqueueAsync(message, cancellationToken);
    }

}