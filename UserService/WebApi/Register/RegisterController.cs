using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Register;
using UserService.Application.Register.Dto;
using UserService.WebApi.Register.Contracts;

namespace UserService.WebApi.Register;

[ApiController]
[Route("api")]
public class RegisterController(
    IMapper mapper, 
    IRegisterOrchestrator registerOrchestrator) : ControllerBase
{
    [HttpPost("register")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> RegisterAsync([Required, FromForm] RegisterContract registerContract)
    {
        var registerDto = mapper.Map<RegisterDto>(registerContract);
        var result = await registerOrchestrator.RegisterUserAsync(registerDto);

        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    } 
    
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync(
        [FromQuery] string token,
        [FromQuery] string nickname)
    {
        var result = await registerOrchestrator
            .EmailConfirmationAsync(token, nickname);

        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    }
}