namespace UserService.WebApi.Auth.Settings;

public class AuthCookieSettings
{
    public string Name { get; init; }
    public SameSiteMode SameSite { get; init; } = SameSiteMode.Strict;
}