using UserService.Application.Login.Dto;

namespace UserService.Application.Login;

public interface ILoginOrchestrator
{
    public Task<OperationResult<string>> LoginAsync(LoginDto loginDto);
}