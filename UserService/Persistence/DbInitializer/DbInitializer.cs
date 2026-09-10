using Microsoft.EntityFrameworkCore;

namespace UserService.Persistence.DbInitializer;

public class DbInitializer(
    AppDbContext db, 
    ILogger<DbInitializer> logger) : IDbInitializer
{
    public async Task Initialize()
    {
        try
        {
            if ((await db.Database.GetPendingMigrationsAsync()).Any())
            {
                await db.Database.MigrateAsync();
            }
        }
        catch
        {
            logger.LogInformation("Error with migration");
        }
    }
}