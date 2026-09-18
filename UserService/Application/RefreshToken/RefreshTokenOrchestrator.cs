using UserService.Core.RefreshToken;
using UserService.Core.User;
using UserService.Infrastructure.Jwt;

namespace UserService.Application.RefreshToken;

public class RefreshTokenOrchestrator(
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    IJwtService jwtService) : IRefreshTokenOrchestrator
{
    public async Task<OperationResult<TokenPair>> RefreshAsync(string rawRefreshToken)
    {
        var hash = jwtService.HashRefreshToken(rawRefreshToken);
        var existing = await refreshTokenRepository.GetByHashAsync(hash);

        if (existing is null || !existing.IsActive)
            return OperationResult<TokenPair>.Fail("Invalid or expired refresh token");

        var user = await userRepository.GetUserByIdAsync(existing.UserId);
        if (user is null)
            return OperationResult<TokenPair>.Fail("User not found");
        
        existing.Revoke();
        await refreshTokenRepository.UpdateAsync(existing);

        var newAccessToken = jwtService.GenerateAccessToken(user.Id, user.NickName);
        var newRawRefreshToken = jwtService.GenerateRefreshTokenValue();
        var newHash = jwtService.HashRefreshToken(newRawRefreshToken);
        var newRefreshToken = RefreshTokenModel.Create(
            user.Id, newHash, jwtService.GetRefreshTokenLifetime());

        await refreshTokenRepository.AddAsync(newRefreshToken);

        return OperationResult<TokenPair>.Ok(new TokenPair(newAccessToken, newRawRefreshToken));
    }
}