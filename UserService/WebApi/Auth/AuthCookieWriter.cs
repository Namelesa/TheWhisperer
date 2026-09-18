using Microsoft.Extensions.Options;
using UserService.Infrastructure.Jwt;
using UserService.WebApi.Auth.Settings;

namespace UserService.WebApi.Auth;

public class AuthCookieWriter(
    IOptions<AuthCookieSettings> cookieOptions,
    IOptions<JwtSettings> jwtOptions) : IAuthCookieWriter
{
    private readonly AuthCookieSettings _cookieSettings = cookieOptions.Value;
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public void SetAccessTokenCookie(HttpResponse response, string token)
    {
        response.Cookies.Append(_cookieSettings.Name, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = _cookieSettings.SameSite,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.AccessTokenMinutes)
        });
    }

    public void RemoveAccessTokenCookie(HttpResponse response)
    {
        response.Cookies.Delete(_cookieSettings.Name);
    }
}