using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.User;
using UserService.Application.User.Dto;
using UserService.WebApi.User.Contracts;
using System.ComponentModel.DataAnnotations;

namespace UserService.WebApi.User;

[ApiController]
[Route("api")]
public class UserController(
    IMapper mapper,
    IUserOrchestrator userOrchestrator) : ControllerBase
{
    [HttpPatch("edit-user")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateUserAsync(
        [Required, FromForm] EditUserContract editUserContract, 
        [FromQuery] string nickName)
    {
        var editDto = mapper.Map<EditUserDto>(editUserContract);
        var result = await userOrchestrator.UpdateUserAsync(editDto, nickName);

        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    } 
    
    [HttpPatch("edit-user-password")]
    public async Task<IActionResult> UpdateUserPasswordAsync(
        [Required, FromForm] EditUserPasswordContract editUserPasswordContract)
    {
        var editDto = mapper.Map<EditUserPasswordDto>(editUserPasswordContract);
        var result = await userOrchestrator.UpdateUserPasswordAsync(editDto);

        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    } 
    
    [HttpDelete("delete-user")]
    public async Task<IActionResult> DeleteUserAsync(string nickName)
    {
        var result = await userOrchestrator.DeleteUserAsync(nickName);

        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    }     
}