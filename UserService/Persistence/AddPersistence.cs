using Microsoft.EntityFrameworkCore;
using UserService.Core.EmailConfirmation;
using UserService.Core.User;
using UserService.Persistence.DbInitializer;
using UserService.Persistence.User;
using UserService.Persistence.UserEmailConfirmation;

namespace UserService.Persistence;

public static class AddPersistence
{
    public static void AddPersistenceLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserEmailConfirmationRepository, UserEmailConfirmationRepository>();
        services.AddScoped<IDbInitializer, DbInitializer.DbInitializer>();
    }
}