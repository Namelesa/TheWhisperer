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

        if (userDto.Token == null)
        {
            var email = decryptInfo.Decrypt(userDto.Email);
            var nickName = decryptInfo.Decrypt(userDto.NickName);
            
            userDto.SetNickNameAndEmail(nickName, email);
            
            var result = await userInfoOrchestrator.SendEditUserInfoEmailAsync(userDto);
    
            if (!result.Success)
            {
                logger.LogWarning("Failed to send confirmation email: {Message}", result.Message);
                throw new InvalidOperationException(result.Message);
            }
        }
        else
        {
            decryptInfo.DecryptObjectStrings(userDto);
        
            var result = await userInfoOrchestrator.SendConfirmEmailAsync(userDto);
            
            if (!result.Success)
            {
                logger.LogWarning("Failed to send confirmation email: {Message}", result.Message);
                throw new InvalidOperationException(result.Message);
            }
        }
    }
}