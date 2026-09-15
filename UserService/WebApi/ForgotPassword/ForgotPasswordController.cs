using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.PasswordRecovery;
using UserService.Application.PasswordRecovery.Dto;
using UserService.WebApi.ForgotPassword.Contracts;

namespace UserService.WebApi.ForgotPassword;

[ApiController]
[Route("api")]
public class ForgotPasswordController(
    IMapper mapper,
    IPasswordRecoveryOrchestrator passwordRecoveryOrchestrator) : ControllerBase
{
    [HttpPatch("forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync(
        [FromForm][Required] ForgotPasswordContract forgotPasswordContract)
    {
        var forgotPasswordDto = mapper.Map<ForgotPasswordDto>(forgotPasswordContract);
        var result = await passwordRecoveryOrchestrator.ForgotPasswordAsync(forgotPasswordDto);

        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    } 
    
    [HttpPatch("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(
        [FromForm][Required] ResetPasswordContract resetPasswordContract)
    {
        var resetPasswordDto = mapper.Map<ResetPasswordDto>(resetPasswordContract);
        var result = await passwordRecoveryOrchestrator.ResetPasswordAsync(resetPasswordDto);

        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    } 
}