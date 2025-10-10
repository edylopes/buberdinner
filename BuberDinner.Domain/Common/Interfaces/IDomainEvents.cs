

using MediatR;

namespace BuberDinner.Domain.Common.Interfaces;

public interface IDomainEvent : INotification
{
    public DateTime OccurredOn { get; }
}
