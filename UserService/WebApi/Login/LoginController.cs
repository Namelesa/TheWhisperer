using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Login;
using UserService.Application.Login.Dto;
using UserService.WebApi.Auth;
using UserService.WebApi.Login.Contracts;

namespace UserService.WebApi.Login;

[ApiController]
[Route("api")]
public class LoginController(
    IMapper mapper, 
    ILoginOrchestrator loginOrchestrator,
    IAuthCookieWriter authCookieWriter) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([Required, FromForm] LoginContract loginContract)
    {
        var loginDto = mapper.Map<LoginDto>(loginContract);
        var result = await loginOrchestrator.LoginAsync(loginDto);

        if (!result.Success)
            return BadRequest(new { message = result.Message });

        authCookieWriter.SetAuthCookies(Response, result.Data!.AccessToken, result.Data.RefreshToken);

        return Ok(new { message = "Logged in successfully" });
    }
}