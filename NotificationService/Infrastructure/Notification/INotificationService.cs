using NotificationService.Core.UserDelete;
using NotificationService.Core.UserInfo;

namespace NotificationService.Infrastructure.Notification;

public interface INotificationService
{
    Task<bool> SendConfirmEmailAsync(UserInfoModel userDto, string link);
    Task<bool> SendEditUserInfoEmailAsync(UserInfoModel userDto);
    Task<bool> SendDeleteUserEmailAsync(UserDeleteModel userDto);
}