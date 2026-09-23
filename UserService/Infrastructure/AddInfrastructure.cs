using Encryptor.Decryption;
using Encryptor.Encryption;
using MassTransit;
using Microsoft.Extensions.Options;
using UserService.Infrastructure.EmailConfirmation;
using UserService.Infrastructure.ForgotPassword;
using UserService.Infrastructure.HasherPassword;
using UserService.Infrastructure.HasherUser;
using UserService.Infrastructure.Jwt;
using UserService.Infrastructure.MessageBroker;
using UserService.Infrastructure.RefreshTokenCleanup;
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
        services.AddHostedService<RefreshTokenCleanupService>();
        
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection("Jwt"))
            .Validate(s => !string.IsNullOrEmpty(s.Key), "Jwt:Key must be configured")
            .Validate(s => !string.IsNullOrEmpty(s.Issuer), "Jwt:Issuer must be configured")
            .ValidateOnStart();
        
        services.AddScoped<IJwtService, JwtService>();
        services.AddSingleton<IEncryptInfo, EncryptInfo>();
        services.AddSingleton<IDecryptInfo, DecryptInfo>();
        
        services.Configure<MessageBrokerSettings>(
            configuration.GetSection("MessageBroker"));

        services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

        services.AddMassTransit(busConfiguration =>
        {
            //busConfiguration.AddConsumer<PublicKeyConsumer>();
    
            busConfiguration.UsingRabbitMq((context, configurator) =>
            {
                var settings = context.GetRequiredService<MessageBrokerSettings>();
         
                configurator.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.UserName);
                    h.Password(settings.Password);
                });
        
                /*configurator.ReceiveEndpoint("confirm-email-queue", e =>
                {
                    e.ConfigureConsumer<ConfirmEmailConsumer>(context);
                });*/
            });
        });
    }
}