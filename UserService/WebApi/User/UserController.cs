using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.User;
using UserService.Application.User.Dto;
using UserService.WebApi.User.Contracts;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using UserService.WebApi.Auth;

namespace UserService.WebApi.User;

[Authorize]
[ApiController]
[Route("api")]
public class UserController(
    IMapper mapper,
    IUserOrchestrator userOrchestrator,
    IAuthCookieWriter authCookieWriter) : ControllerBase
{
    [HttpPatch("edit-user")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateUserAsync(
        [Required, FromForm] EditUserContract editUserContract)
    {
        var userId = GetUserIdFromClaims();
        var editDto = mapper.Map<EditUserDto>(editUserContract);
        var result = await userOrchestrator.UpdateUserAsync(editDto, userId);

        if(!result.Success)
            return BadRequest(new { message = result.Message });
        
        authCookieWriter.RemoveAuthCookies(Response);

        return Ok(new { message = result.Data });
    } 
    
    [HttpDelete("delete-user")]
    public async Task<IActionResult> DeleteUserAsync()
    {
        var userId = GetUserIdFromClaims();
        var result = await userOrchestrator.DeleteUserAsync(userId);
        
        if(!result.Success)
            return BadRequest(new { message = result.Message });
        
        authCookieWriter.RemoveAuthCookies(Response);
        
        return Ok(new { message = result.Data });
    }     
    
    [HttpGet("get-user")]
    public async Task<IActionResult> GetUserAsync()
    {
        var userId = GetUserIdFromClaims();
        var result = await userOrchestrator.GetUserByIdAsync(userId);

        if(!result.Success)
            return BadRequest(new { message = result.Message });

        authCookieWriter.RemoveAuthCookies(Response);
        
        return Ok(new { message = result.Data });
    }
    
    private Guid GetUserIdFromClaims()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new Exception("User ID claim not found.");
        }

        return Guid.Parse(userIdClaim.Value);
    }
}