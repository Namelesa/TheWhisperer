using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.PasswordRecovery;
using UserService.Application.PasswordRecovery.Dto;
using UserService.WebApi.Auth;
using UserService.WebApi.ForgotPassword.Contracts;

namespace UserService.WebApi.ForgotPassword;

[Authorize]
[ApiController]
[Route("api")]
public class ForgotPasswordController(
    IMapper mapper,
    IPasswordRecoveryOrchestrator passwordRecoveryOrchestrator,
    IAuthCookieWriter authCookieWriter) : ControllerBase
{
    [HttpPatch("forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync(
        [FromForm][Required] ForgotPasswordContract forgotPasswordContract)
    {
        var forgotPasswordDto = mapper.Map<ForgotPasswordDto>(forgotPasswordContract);
        var result = await passwordRecoveryOrchestrator.ForgotPasswordAsync(forgotPasswordDto);

        if(!result.Success)
            return BadRequest(new { message = result.Message });
        
        authCookieWriter.RemoveAuthCookies(Response);
        
        return Ok(new { message = result.Data });
    } 
    
    [HttpPatch("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(
        [FromForm][Required] ResetPasswordContract resetPasswordContract)
    {
        var resetPasswordDto = mapper.Map<ResetPasswordDto>(resetPasswordContract);
        var result = await passwordRecoveryOrchestrator.ResetPasswordAsync(resetPasswordDto);

        if(!result.Success)
            return BadRequest(new { message = result.Message });
        
        authCookieWriter.RemoveAuthCookies(Response);
        
        return Ok(new { message = result.Data });
    } 
}