using MassTransit;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using NotificationService.Infrastructure.MailJet;
using NotificationService.Infrastructure.MessageBroker;
using NotificationService.Infrastructure.MessageBroker.Consumers;
using NotificationService.Infrastructure.Notification;
using NotificationService.Infrastructure.TemplateReader;

namespace NotificationService.Infrastructure;

public static class AddInfrastructure
{
    public static void AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<INotificationService, Notification.NotificationService>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<ITemplateReader, TemplateReader.TemplateReader>();
        
        services.Configure<MessageBrokerSettings>(
            configuration.GetSection("MessageBroker"));

        services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

        services.AddMassTransit(busConfiguration =>
        {
            busConfiguration.AddConsumer<UserEmailConfirmConsumer>();
            busConfiguration.AddConsumer<UserDeleteConsumer>();
    
            busConfiguration.UsingRabbitMq((context, configurator) =>
            {
                var settings = context.GetRequiredService<MessageBrokerSettings>();
         
                configurator.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.UserName);
                    h.Password(settings.Password);
                });
        
                configurator.ReceiveEndpoint("confirm-email-queue", e =>
                {
                    e.ConfigureConsumer<UserEmailConfirmConsumer>(context);
                });
                
                configurator.ReceiveEndpoint("delete-user-queue", e =>
                {
                    e.ConfigureConsumer<UserDeleteConsumer>(context);
                });
            });
        });
    }
}