using Encryptor.Decryption;
using Encryptor.Encryption;
using UserService.Infrastructure.EmailConfirmation;
using UserService.Infrastructure.ForgotPassword;
using UserService.Infrastructure.HasherPassword;
using UserService.Infrastructure.HasherUser;
using UserService.Infrastructure.Jwt;
using UserService.Infrastructure.UnconfirmedUserCleanup;

namespace UserService.Infrastructure;

public static class AddInfrastructure
{
    public static void AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var base64Key = configuration["Security:HmacKey"]
                        ?? throw new InvalidOperationException(
                            "Security:HmacKey is not configured.");

        services.AddSingleton<IHasherUser>(
            HasherUser.HasherUser.FromBase64Key(base64Key));
        
        services.AddSingleton<IHasherPassword, HasherPassword.HasherPassword>();
        services.AddSingleton<IEmailConfirmationService, EmailConfirmationService>();
        services.AddSingleton<IUserForgotPasswordService, UserForgotPasswordService>();
        services.AddHostedService<UnconfirmedUsersCleanupService>();
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddScoped<IJwtService, JwtService>();
        // services.AddHostedService<UnconfirmedUsersCleanupService>();
        services.AddSingleton<IEncryptInfo, EncryptInfo>();
        services.AddSingleton<IDecryptInfo, DecryptInfo>();
    }
}