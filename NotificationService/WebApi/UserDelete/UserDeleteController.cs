using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.UserInfo;
using NotificationService.Core.UserDelete;
using NotificationService.WebApi.UserDelete.Contracts;

namespace NotificationService.WebApi.UserDelete;

[ApiController]
[Route("api")]
public class UserDeleteController(
    IUserInfoOrchestrator userInfoOrchestrator,
    IMapper mapper) : ControllerBase
{
    [HttpPost("delete-user-email")]
    public async Task<IActionResult> SendDeleteUserEmailAsync([FromForm] UserDeleteContract userDeleteContract)
    {
        var userDeleteModel = mapper.Map<UserDeleteModel>(userDeleteContract);
        var result = await userInfoOrchestrator.SendDeleteUserInfoEmailAsync(userDeleteModel);
       
        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    }
}