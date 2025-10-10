
using BuberDinner.Application.Authentication.Queries;
using BuberDinner.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly.Retry;
namespace BuberDinner.Application.Authentication.Common.Beahviors;

using BuberDinner.Domain.Common;

using Microsoft.EntityFrameworkCore;
using OneOf;
using Polly;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
where TResponse : IOneOf
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
    private readonly IPublisher _publisher;
    public TransactionBehavior(IUnitOfWork uow,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger,
    IPublisher _publisher
    )
    {
        this._uow = uow;
        this._logger = logger;
        this._publisher = _publisher;
    }

    public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
    {
        if (request is IQuery<TResponse>)
            return await next(); // Pula transação

        _logger.LogInformation("Opening transaction for {RequestName}", typeof(TRequest).Name);

        await _uow.BeginTransactionAsync(cancellationToken); // Abre transação ANTES do handler

        try
        {
            var response = await next(); // Executa o handler dentro da transação (loggin neste caso)

            await GetRetryPolicy().ExecuteAsync(async () =>
           {
               await _uow.CommitAsync(cancellationToken); // Retry apenas aqui

               var domainEvents = _uow.CollectDomainEvents();

               foreach (var domainEvent in domainEvents)
                   await _publisher.Publish(domainEvent, cancellationToken);

           });

            _logger.LogInformation("Transaction committed for {RequestName}", typeof(TRequest).Name);

            return response;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency exception on {RequestName}, rolling back", typeof(TRequest).Name);
            await _uow.RollbackAsync(cancellationToken);
            throw; //trown to filter/middleware
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error on {RequestName}, rolling back", typeof(TRequest).Name);
            await _uow.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private AsyncRetryPolicy GetRetryPolicy()
    {
        return Policy
            .Handle<DbUpdateConcurrencyException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200 * attempt),
                onRetry: (ex, delay, attempt, context) =>
                {
                    _logger.LogWarning("Retry {Attempt} due to concurrency conflict. Waiting {Delay}ms", attempt, delay.TotalMilliseconds);
                });
    }

    private async Task PublishEventsHandler(CancellationToken ct)
    {
        var entitiesWithEvents = _uow.ChangeTracker
             .Entries<Entity>()
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
