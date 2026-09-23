using NotificationService.Core.UserDelete;
using NotificationService.Core.UserInfo;

namespace NotificationService.Application.UserInfo;

public interface IUserInfoOrchestrator
{
    Task<OperationResult<string>> SendConfirmEmailAsync(UserInfoModel userDto);
    Task<OperationResult<string>> SendEditUserInfoEmailAsync(UserInfoModel userDto);
    Task<OperationResult<string>> SendDeleteUserInfoEmailAsync(UserDeleteModel userDto);
}