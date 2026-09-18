namespace UserService.WebApi.Auth;

public interface IAuthCookieWriter
{
    void SetAccessTokenCookie(HttpResponse response, string token);
    void RemoveAccessTokenCookie(HttpResponse response);
}