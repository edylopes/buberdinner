
using BuberDinner.Application.Authentication.Queries;
using BuberDinner.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly.Retry;
namespace BuberDinner.Application.Authentication.Common.Beahviors;

using Microsoft.EntityFrameworkCore;
using Polly;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(IUnitOfWork uow, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _uow = uow;
        _logger = logger;
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
            var response = await next(); // Executa o handler dentro da transação

            await GetRetryPolicy().ExecuteAsync(async () =>
           {
               await _uow.CommitAsync(cancellationToken, detectChange: true); // Retry apenas aqui
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
}
