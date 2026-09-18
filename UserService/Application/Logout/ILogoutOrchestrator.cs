namespace UserService.Application.Logout;

public interface ILogoutOrchestrator
{
    Task<OperationResult<string>> LogoutAsync(string rawRefreshToken);
}