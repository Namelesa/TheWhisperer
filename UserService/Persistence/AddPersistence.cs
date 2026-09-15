using Microsoft.EntityFrameworkCore;
using UserService.Core.User;
using UserService.Core.EmailConfirmation;
using UserService.Core.ForgotPassword;
using UserService.Persistence.User;
using UserService.Persistence.UserEmailConfirmation;
using UserService.Persistence.UserForgotPassword;
using UserService.Persistence.DbInitializer;

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
        services.AddScoped<IUserForgotPasswordRepository, UserForgotPasswordRepository>();
        services.AddScoped<IDbInitializer, DbInitializer.DbInitializer>();
    }
}