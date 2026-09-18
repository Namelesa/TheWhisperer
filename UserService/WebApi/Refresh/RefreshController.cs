using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserService.Application.RefreshToken;
using UserService.WebApi.Auth;
using UserService.WebApi.Auth.Settings;

namespace UserService.WebApi.Refresh;

[ApiController]
[Route("api")]
public class RefreshController(
    IRefreshTokenOrchestrator refreshOrchestrator,
    IAuthCookieWriter authCookieWriter,
    IOptions<AuthCookieSettings> cookieOptions) : ControllerBase
{
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync()
    {
        var refreshToken = Request.Cookies[cookieOptions.Value.RefreshTokenName];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(new { message = "Refresh token is missing" });

        var result = await refreshOrchestrator.RefreshAsync(refreshToken);

        if (!result.Success)
        {
            authCookieWriter.RemoveAuthCookies(Response);
            return Unauthorized(new { message = result.Message });
        }

        authCookieWriter.SetAuthCookies(Response, result.Data!.AccessToken, result.Data.RefreshToken);

        return Ok(new { message = "Token refreshed successfully" });
    }
}