
using BuberDinner.Infrastructure.Persistence.Context;

namespace BuberDinner.Infrastructure.Persistence;

public class MigrationHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MigrationHostedService> _logger;
    private const int MaxRetries = 10;
    private const int InitialDelayMs = 1000;
    private const int MaxDelayMs = 10000;

    public MigrationHostedService(IServiceProvider serviceProvider, ILogger<MigrationHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔄 Iniciando aplicação de migrations com retry exponencial...");

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        int attempt = 0;
        int delay = InitialDelayMs;

        while (attempt < MaxRetries)
        {
            try
            {
                await context.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("✅ Migrations applied or the database is already up to date!");

                return;
            }
            catch (Exception ex)
            {
                attempt++;
                _logger.LogWarning(ex, $"⚠️ Tentativa {attempt}/{MaxRetries} falhou. Próxima tentativa em {delay / 1000.0}s");

                if (attempt >= MaxRetries)
                {
                    _logger.LogError(ex, "❌ Não foi possível aplicar migrations após várias tentativas.");
                    throw; // Decide se quer parar a aplicação ou apenas logar
                }

                // Delay exponencial com limite
                await Task.Delay(delay, cancellationToken);
                delay = Math.Min(delay * 2, MaxDelayMs);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}