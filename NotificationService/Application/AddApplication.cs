using Encryptor.Decryption;
using NotificationService.Application.UserInfo;

namespace NotificationService.Application;

public static class AddApplication
{
    public static void AddApplicationLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IUserInfoOrchestrator, UserInfoOrchestrator>();
        services.AddSingleton<IDecryptInfo, DecryptInfo>();
    }
}