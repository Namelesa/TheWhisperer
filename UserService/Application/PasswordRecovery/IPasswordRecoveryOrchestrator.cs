using UserService.Application.PasswordRecovery.Dto;

namespace UserService.Application.PasswordRecovery;

public interface IPasswordRecoveryOrchestrator
{
    Task<OperationResult<string>> ForgotPasswordAsync(
        ForgotPasswordDto forgotPasswordDto);

    Task<OperationResult<string>> ResetPasswordAsync(
        ResetPasswordDto resetPasswordDto);
}