using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.UserInfo;
using NotificationService.Core.UserInfo;

namespace NotificationService.WebApi.UserInfo;

[ApiController]
[Route("api")]
public class UserInfoController(
    IUserInfoOrchestrator userInfoOrchestrator) : ControllerBase
{
    [HttpPost("confirm-user-email")]
    public async Task<IActionResult> SendConfirmEmailAsync([FromForm] UserInfoModel userInfoModel)
    { 
        var result = await userInfoOrchestrator.SendConfirmEmailAsync(userInfoModel);
       
        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    }
    
    [HttpPost("edit-user-email")]
    public async Task<IActionResult> SendEditUserInfoEmailAsync([FromForm] UserInfoModel userInfoModel)
    {
        var result = await userInfoOrchestrator.SendEditUserInfoEmailAsync(userInfoModel);
       
        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    }
}