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

    public void SetAuthCookies(HttpResponse response, string accessToken, string refreshToken)
    {
        response.Cookies.Append(_cookieSettings.Name, accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = _cookieSettings.SameSite,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.AccessTokenMinutes)
        });

        response.Cookies.Append(_cookieSettings.RefreshTokenName, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = _cookieSettings.SameSite,
            Path = "/api/refresh",
            Expires = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenDays)
        });
    }
    
    public void RemoveAuthCookies(HttpResponse response)
    {
        response.Cookies.Delete(_cookieSettings.Name);
        response.Cookies.Delete(_cookieSettings.RefreshTokenName, new CookieOptions
        {
            Path = "/api/refresh"
        });
    }
}