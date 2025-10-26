using BuberDinner.Application.Common.Extensions;
using BuberDinner.Application.Common.Interfaces;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;
using Polly;
using Polly.Retry;

namespace BuberDinner.Application.Common.Beahviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
where TResponse : IOneOf
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
    private readonly IPublisher _publisher;
    public TransactionBehavior(IUnitOfWork uow,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger,
    IPublisher publisher
    )
    {
        this._uow = uow;
        this._logger = logger;
        this._publisher = publisher;
    }
    public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
    {
        if (request is IQuery<TResponse>)
            return await next(); // Skip transaction for queries

        try
        {
            _logger.LogInformation("Opening transaction for {RequestName}", typeof(TRequest).Name);
            await _uow.BeginTransactionAsync(cancellationToken);
            // First try without transaction to validate
            var response = await next();
            // Only open transaction if success
            if (response.IsSuccess())
            {


                await GetRetryPolicy().ExecuteAsync(async () => //Retry
                {
                    await _uow.CommitAsync(cancellationToken);

                    var domainEvents = _uow.CollectDomainEvents();

                    if (domainEvents.Any())
                        foreach (var domainEvent in domainEvents)
                            await _publisher.Publish(domainEvent, cancellationToken);

                    _logger.LogInformation("Transaction committed for {RequestName}", typeof(TRequest).Name);
                });

            }
            else
            {
                _logger.LogInformation("Skipping transaction: response not successful for {RequestName}", typeof(TRequest).Name);
            }

            return response;
        }
        catch (Exception ex)
        {
            await _uow.RollbackAsync(cancellationToken);
            _logger.LogInformation("Rolling back transaction: response not successful for {RequestName}", typeof(TRequest).Name);
            _logger.LogError(ex, "Unhandled error on {RequestName}", typeof(TRequest).Name);
            throw;
        }
    }
    private AsyncRetryPolicy GetRetryPolicy()
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200 * attempt),
                onRetry: (ex, delay, attempt, context) =>
                {
                    _logger.LogWarning("Retry {Attempt}:  Waiting {Delay}ms", attempt, delay.TotalMilliseconds);
                });
    }

    private async Task PublishEventsHandler(CancellationToken ct)
    {
        var entitiesWithEvents = _uow.ChangeTracker
             .Entries<IHasDomainEvents>()
             .Where(entity => entity.Entity.DomainEvents.Any())
             .Select(e => e.Entity)
             .ToList();

        var domainEvents = entitiesWithEvents.
               SelectMany(e => e.DomainEvents)
              .ToList();

        foreach (var entity in entitiesWithEvents)
            entity.ClearDomainEvents();

        _logger.LogInformation("Publishing {Count} domain events", domainEvents.Count);

        foreach (var domainEvent in domainEvents)
            await _publisher.Publish(domainEvent, ct);

    }
}
