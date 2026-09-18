using UserService.Core.RefreshToken;

namespace UserService.Infrastructure.RefreshTokenCleanup;

public class RefreshTokenCleanupService(
    IServiceScopeFactory scopeFactory,
    ILogger<RefreshTokenCleanupService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromHours(6));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var refreshTokenRepository = scope.ServiceProvider
                    .GetRequiredService<IRefreshTokenRepository>();

                await refreshTokenRepository.DeleteExpiredOrRevokedAsync();
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Refresh token cleanup failed");
            }
        }
    }
}