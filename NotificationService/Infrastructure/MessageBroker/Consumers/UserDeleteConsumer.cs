using AutoMapper;
using Encryptor.Decryption;
using MassTransit;
using NotificationService.Application.UserInfo;
using NotificationService.Core.UserDelete;
using SharedModels.User.UserDelete;

namespace NotificationService.Infrastructure.MessageBroker.Consumers;

public class UserDeleteConsumer(
    IUserInfoOrchestrator userInfoOrchestrator, 
    ILogger<UserEmailConfirmConsumer> logger,
    IMapper mapper,
    IDecryptInfo decryptInfo) : IConsumer<UserDeleteByEmailModel>
{
    public async Task Consume(ConsumeContext<UserDeleteByEmailModel> context)
    {
        var info = context.Message;
        var userDto = mapper.Map<UserDeleteModel>(info);
    
        decryptInfo.DecryptObjectStrings(userDto);
        logger.LogDebug("UserDeleteConsumer: Decrypted user info for user {UserId}", userDto.NickName);
    
        var result = await userInfoOrchestrator.SendDeleteUserInfoEmailAsync(userDto);
    
        if (!result.Success)
        {
            logger.LogWarning("Failed to send confirmation email: {Message}", result.Message);
            throw new InvalidOperationException(result.Message);
        }
    }
}