using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Login;
using UserService.Application.Login.Dto;
using UserService.WebApi.Login.Contracts;

namespace UserService.WebApi.Login;

[ApiController]
[Route("api")]
public class LoginController(
    IMapper mapper, 
    ILoginOrchestrator loginOrchestrator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([Required, FromForm] LoginContract loginContract)
    {
        var loginDto = mapper.Map<LoginDto>(loginContract);
        var result = await loginOrchestrator.LoginAsync(loginDto);

        return result.Success
            ? Ok(new { message = result.Data })
            : BadRequest(new { message = result.Message });
    } 
}