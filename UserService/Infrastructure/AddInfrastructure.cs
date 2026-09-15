using Encryptor.Decryption;
using Encryptor.Encryption;
using UserService.Application.HasherPassword;
using UserService.Application.HasherUser;
using UserService.Infrastructure.EmailConfirmation;
using UserService.Infrastructure.ForgotPassword;
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
        // services.AddHostedService<UnconfirmedUsersCleanupService>();
        services.AddSingleton<IEncryptInfo, EncryptInfo>();
        services.AddSingleton<IDecryptInfo, DecryptInfo>();
    }
}