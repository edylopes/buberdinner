using System.Diagnostics;

using BuberDinner.Application.Common.Beahviors.Logger;

using Microsoft.Extensions.Logging;


namespace BuberDinner.Application.Common.Beahviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IOneOf
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<ILoggingStrategy<TRequest, TResponse>> _strategies;

    public LoggingBehavior(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger,
    IEnumerable<ILoggingStrategy<TRequest, TResponse>> strategies)
    {
        this._logger = logger;
        this._strategies = strategies;
    }
    public async Task<TResponse> Handle(
      TRequest request,
      RequestHandlerDelegate<TResponse> next,
      CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        //  _logger.LogInformation("➡️ Handling {RequestName}: {@Request}", requestName, request);

        var stopwatch = Stopwatch.StartNew();
        var response = await next();//Chama o próximo da cadeia (ou o handler)
        stopwatch.Stop();

        _logger.LogInformation("⬅️ Handled {RequestName} in {Elapsed} ms", requestName, stopwatch.ElapsedMilliseconds);

        foreach (var _strategie in _strategies)
            await _strategie.LogAsync(request, response);

        return response;

    }
}
