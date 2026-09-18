namespace UserService.Infrastructure.Jwt;

public interface IJwtService
{
    string GenerateAccessToken(Guid userId, string nickName);
}