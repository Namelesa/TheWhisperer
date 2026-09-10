using UserService.Core.User;

namespace UserService.Infrastructure.UnconfirmedUserCleanup;

public class UnconfirmedUsersCleanupService(
    IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(
            TimeSpan.FromHours(1));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var userRepository =
                    scope.ServiceProvider
                        .GetRequiredService<IUserRepository>();

                await userRepository.DeleteUnconfirmedUsersAsync();
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"Unconfirmed users cleanup failed: {exception.Message}");
            }
        }
    }
}