namespace UserService.Application.Login;

public class RefreshTokenPolicySettings
{
    public int MaxActiveRefreshTokensPerUser { get; init; } = 5;
}