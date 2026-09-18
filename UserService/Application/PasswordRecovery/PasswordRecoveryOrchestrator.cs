using UserService.Application.PasswordRecovery.Dto;
using UserService.Core.ForgotPassword;
using UserService.Core.User;
using UserService.Infrastructure.ForgotPassword;
using UserService.Infrastructure.HasherPassword;
using UserService.Infrastructure.HasherUser;

namespace UserService.Application.PasswordRecovery;

public class PasswordRecoveryOrchestrator(
    IUserForgotPasswordRepository userForgotPasswordRepository,
    IUserRepository userRepository,
    IUserForgotPasswordService userForgotPasswordService,
    IHasherPassword hasherPassword,
    IHasherUser hasherUser
    ) : IPasswordRecoveryOrchestrator
{
    public async Task<OperationResult<string>> ForgotPasswordAsync(
        ForgotPasswordDto forgotPasswordDto)
    {
        var emailHash = hasherUser.Hash(forgotPasswordDto.Email);
        var user = await userRepository.GetUserByEmailHashAsync(emailHash);

        if (user is null)
            return OperationResult<string>.Fail("User not found");

        await userForgotPasswordRepository.DeleteAllByUserIdAsync(user.Id);

        var token = userForgotPasswordService.GenerateToken();
        var tokenHash = userForgotPasswordService.HashToken(token);
        
        var forgotPassword = new UserForgotPassword(user.Id);
        forgotPassword.SetToken(tokenHash);
        
        await userForgotPasswordRepository.AddAsync(forgotPassword);

        return OperationResult<string>.Ok(token);
    }

    public async Task<OperationResult<string>> ResetPasswordAsync(
        ResetPasswordDto resetPasswordDto)
    {
        var tokenHash = userForgotPasswordService.HashToken(resetPasswordDto.Token);
        var forgotPassword = await userForgotPasswordRepository.GetByTokenHashAsync(tokenHash);

        if (forgotPassword is null 
            || forgotPassword.UsedAt is not null 
            || forgotPassword.ExpiresAt <= DateTime.UtcNow)
            return OperationResult<string>.Fail("Invalid or expired token");

        var user = await userRepository.GetUserByIdAsync(forgotPassword.UserId);

        if (user is null 
            && user?.SecretWortHash != hasherUser.Hash(resetPasswordDto.SecretWort))
            return OperationResult<string>.Fail("Invalid or expired token");

        var newPasswordHash = hasherPassword.Hash(resetPasswordDto.NewPassword);

        user.UpdatePassword(newPasswordHash);
        forgotPassword.MarkAsUsed();

        await userRepository.UpdateUserAsync(user);
        await userForgotPasswordRepository.UpdateAsync(forgotPassword);

        return OperationResult<string>.Ok("Password successfully changed");
    }
}