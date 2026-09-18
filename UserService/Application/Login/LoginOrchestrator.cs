using FluentValidation;
using Microsoft.Extensions.Options;
using UserService.Application.Login.Dto;
using UserService.Application.RefreshToken;
using UserService.Core.RefreshToken;
using UserService.Core.User;
using UserService.Infrastructure.HasherPassword;
using UserService.Infrastructure.HasherUser;
using UserService.Infrastructure.Jwt;

namespace UserService.Application.Login;

public class LoginOrchestrator(
    IUserRepository userRepository, 
    IHasherUser hasherUser, 
    IHasherPassword hasherPassword,
    IJwtService jwtService,
    IValidator<LoginDto> validator,
    IRefreshTokenRepository refreshTokenRepository,
    IOptions<RefreshTokenPolicySettings> refreshTokenPolicyOptions) : ILoginOrchestrator
{
    
    private readonly RefreshTokenPolicySettings _refreshTokenPolicy = refreshTokenPolicyOptions.Value;
    
    public async Task<OperationResult<TokenPair>> LoginAsync(LoginDto loginDto)
    {
        var validationResult = await validator.ValidateAsync(loginDto);
        if (!validationResult.IsValid)
            return OperationResult<TokenPair>.Fail(
                string.Join("; ", validationResult.Errors));

        var hashedNickName = hasherUser.Hash(loginDto.NickName);
        var user = await userRepository.GetUserByNickNameHashAsync(hashedNickName);

        if (user is null)
            return OperationResult<TokenPair>.Fail("User with this nick name does not exist");

        if (!user.ConfirmedEmail)
            return OperationResult<TokenPair>.Fail("Email is not confirmed");

        if (!hasherPassword.Verify(loginDto.Password, user.PasswordHash))
            return OperationResult<TokenPair>.Fail("Incorrect password");

        await EnforceRefreshTokenLimitAsync(user.Id);
        
        var accessToken = jwtService.GenerateAccessToken(user.Id, user.NickName);

        var rawRefreshToken = jwtService.GenerateRefreshTokenValue();
        var refreshTokenHash = jwtService.HashRefreshToken(rawRefreshToken);
        var refreshToken = RefreshTokenModel.Create(
            user.Id, refreshTokenHash, jwtService.GetRefreshTokenLifetime());

        await refreshTokenRepository.AddAsync(refreshToken);

        return OperationResult<TokenPair>.Ok(new TokenPair(accessToken, rawRefreshToken));
    }
    
    private async Task EnforceRefreshTokenLimitAsync(Guid userId)
    {
        var activeTokens = await refreshTokenRepository.GetActiveByUserIdAsync(userId);
        var excessCount = activeTokens.Count - _refreshTokenPolicy.MaxActiveRefreshTokensPerUser + 1;

        if (excessCount <= 0)
            return;

        foreach (var token in activeTokens.Take(excessCount))
        {
            token.Revoke();
            await refreshTokenRepository.UpdateAsync(token);
        }
    }
}