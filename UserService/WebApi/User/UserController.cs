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
        [FromQuery] Guid userId)
    {
        var editDto = mapper.Map<EditUserDto>(editUserContract);
        var result = await userOrchestrator.UpdateUserAsync(editDto, userId);

        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    } 
    
    [HttpPatch("edit-user-password")]
    public async Task<IActionResult> UpdateUserPasswordAsync(
        [Required, FromForm] EditUserPasswordContract editUserPasswordContract, Guid userId)
    {
        var editDto = mapper.Map<EditUserPasswordDto>(editUserPasswordContract);
        var result = await userOrchestrator.UpdateUserPasswordAsync(editDto, userId);

        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    } 
    
    [HttpDelete("delete-user")]
    public async Task<IActionResult> DeleteUserAsync(Guid userId)
    {
        var result = await userOrchestrator.DeleteUserAsync(userId);

        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    }     
    
    [HttpGet("get-user")]
    public async Task<IActionResult> GetUserAsync(Guid userId)
    {
        var result = await userOrchestrator.GetUserByIdAsync(userId);

        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    }
}