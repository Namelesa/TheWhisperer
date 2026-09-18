using UserService.Application.Login.Dto;
using UserService.Application.RefreshToken;

namespace UserService.Application.Login;

public interface ILoginOrchestrator
{
    public Task<OperationResult<TokenPair>> LoginAsync(LoginDto loginDto);
}