

using BuberDinner.Domain.Common.Interfaces;

namespace BuberDinner.Domain.Events.Interfaces;

public interface IHasDomainEvents
{
    IReadOnlyList<IDomainEvent> domainEvents { get; }
    void AddDomainEvent(IDomainEvent domainEvent);
    void ClearDomainEvents();

}
