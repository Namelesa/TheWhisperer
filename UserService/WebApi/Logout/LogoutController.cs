using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserService.Application.Logout;
using UserService.WebApi.Auth;
using UserService.WebApi.Auth.Settings;

namespace UserService.WebApi.Logout;

[ApiController]
[Route("api")]
public class LogoutController(
    ILogoutOrchestrator logoutOrchestrator,
    IAuthCookieWriter authCookieWriter,
    IOptions<AuthCookieSettings> cookieOptions) : ControllerBase
{
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        var refreshTokenValue = Request.Cookies[cookieOptions.Value.RefreshTokenName];

        await logoutOrchestrator.LogoutAsync(refreshTokenValue ?? string.Empty);

        authCookieWriter.RemoveAuthCookies(Response);

        return Ok(new { message = "Logged out successfully" });
    }
}