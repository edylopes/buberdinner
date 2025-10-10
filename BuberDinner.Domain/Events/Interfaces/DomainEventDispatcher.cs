

using System.Collections;

namespace BuberDinner.Domain.Events.Interfaces;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Dispatch(IDomainEvent domainEvent)
    {
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
        var handlers = (IEnumerable)_serviceProvider.GetService(handlerType)!;

        foreach (dynamic handler in handlers)
            await handler.Handle((dynamic)domainEvent);
    }
}
