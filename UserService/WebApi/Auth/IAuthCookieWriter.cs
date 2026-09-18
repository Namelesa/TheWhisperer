namespace UserService.WebApi.Auth;

public interface IAuthCookieWriter
{
    void SetAuthCookies(HttpResponse response, string accessToken, string refreshToken);
    void RemoveAuthCookies(HttpResponse response);
}