namespace UserService.Infrastructure.Jwt;

public interface IJwtService
{
    string GenerateAccessToken(Guid userId, string nickName);
    string GenerateRefreshTokenValue();
    string HashRefreshToken(string rawToken);
    TimeSpan GetRefreshTokenLifetime();
}