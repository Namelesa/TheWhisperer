using UserService.Core.RefreshToken;
using UserService.Infrastructure.Jwt;

namespace UserService.Application.Logout;

public class LogoutOrchestrator(
    IRefreshTokenRepository refreshTokenRepository,
    IJwtService jwtService) : ILogoutOrchestrator
{
    public async Task<OperationResult<string>> LogoutAsync(string rawRefreshToken)
    {
        if (string.IsNullOrEmpty(rawRefreshToken))
            return OperationResult<string>.Ok("Logged out");

        var hash = jwtService.HashRefreshToken(rawRefreshToken);
        var existing = await refreshTokenRepository.GetByHashAsync(hash);

        if (existing is null) return OperationResult<string>.Ok("Logged out");
        existing.Revoke();
        await refreshTokenRepository.UpdateAsync(existing);

        return OperationResult<string>.Ok("Logged out");
    }
}