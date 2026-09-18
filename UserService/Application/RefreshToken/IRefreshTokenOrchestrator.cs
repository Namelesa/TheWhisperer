namespace UserService.Application.RefreshToken;

public interface IRefreshTokenOrchestrator
{
    Task<OperationResult<TokenPair>> RefreshAsync(string rawRefreshToken);
}