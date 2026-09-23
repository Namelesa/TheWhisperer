using AutoMapper;
using Encryptor.Decryption;
using MassTransit;
using NotificationService.Application.UserInfo;
using NotificationService.Core.UserInfo;
using SharedModels.User.UserConfirmEmail;

namespace NotificationService.Infrastructure.MessageBroker.Consumers;

public class UserEmailConfirmConsumer(
    IUserInfoOrchestrator userInfoOrchestrator, 
    ILogger<UserEmailConfirmConsumer> logger,
    IMapper mapper,
    IDecryptInfo decryptInfo) : IConsumer<UserEmailConfirmModel>
{
    public async Task Consume(ConsumeContext<UserEmailConfirmModel> context)
    {
        var info = context.Message;
        var userDto = mapper.Map<UserInfoModel>(info);
    
        decryptInfo.DecryptObjectStrings(userDto);
    
        var result = await userInfoOrchestrator.SendConfirmEmailAsync(userDto);
    
        if (!result.Success)
        {
            logger.LogWarning("Failed to send confirmation email: {Message}", result.Message);
            throw new InvalidOperationException(result.Message);
        }
    }
}