using Encryptor.Decryption;
using Encryptor.Encryption;
using UserService.Application.EmailConfirmation;
using UserService.Application.HasherPassword;
using UserService.Application.HasherUser;
using UserService.Infrastructure.EmailConfirmation;
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
        services.AddHostedService<UnconfirmedUsersCleanupService>();
        services.AddSingleton<IEncryptInfo, EncryptInfo>();
        services.AddSingleton<IDecryptInfo, DecryptInfo>();
    }
}