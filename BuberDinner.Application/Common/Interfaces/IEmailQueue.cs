

using BuberDinner.Contracts.Authentication.Email;

namespace BuberDinner.Application.Common.Interfaces;

public interface IEmailQueue
{
    public void StartConsuming(CancellationToken cancellationToken = default);
    ValueTask EnqueueAsync(EmailMessage message, CancellationToken ct);
    public void Connect();
}
