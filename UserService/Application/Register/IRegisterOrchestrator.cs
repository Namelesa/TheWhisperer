using UserService.Application.Register.Dto;

namespace UserService.Application.Register;

public interface IRegisterOrchestrator
{
    public Task<OperationResult<string>> RegisterUserAsync(RegisterDto registerDto);
    public Task<OperationResult<string>> EmailConfirmationAsync(
        string emailConfirmationToken, 
        string nickname);
}