using NotificationService.Core.UserDelete;
using NotificationService.Core.UserInfo;
using NotificationService.Infrastructure.Notification;

namespace NotificationService.Application.UserInfo;

public class UserInfoOrchestrator(
    INotificationService notification) : IUserInfoOrchestrator
{
    private async Task<OperationResult<string>> SendEmailAsync(UserInfoModel userDto, Func<UserInfoModel, Task<bool>> sendEmail)
    {
        var result = await sendEmail(userDto);
        
        return result
            ? OperationResult<string>.Ok("User notified")
            : OperationResult<string>.Fail("User was not notified");
    }

    public Task<OperationResult<string>> SendConfirmEmailAsync(UserInfoModel userDto)
        => SendEmailAsync(userDto, async u =>
        {
            var link = GenerateLink(userDto.Token, userDto.NickName);
            return await notification.SendConfirmEmailAsync(u, link);
        });

    public Task<OperationResult<string>> SendEditUserInfoEmailAsync(UserInfoModel userDto)
        => SendEmailAsync(userDto, notification.SendEditUserInfoEmailAsync);

    public async Task<OperationResult<string>> SendDeleteUserInfoEmailAsync(UserDeleteModel userDto)
    {
        await notification.SendDeleteUserEmailAsync(userDto);
        return OperationResult<string>.Ok("User notified");
    }
    
    private static string GenerateLink(string token, string nickName)
    {
        const string baseUrl = "https://localhost:7164/api/";
        var encodedToken = Uri.EscapeDataString(token);
        var encodedNickName = Uri.EscapeDataString(nickName);
        return $"{baseUrl}confirm-email?token={encodedToken}&nickname={encodedNickName}";
    }
}