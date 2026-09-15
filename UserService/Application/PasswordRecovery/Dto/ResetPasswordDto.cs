namespace UserService.Application.PasswordRecovery.Dto;

public class ResetPasswordDto(
    string secretWort,
    string token,
    string newPassword)
{
    public string SecretWort { get; init; } = secretWort;
    public string Token { get; init; } = token;
    public string NewPassword { get; init; } = newPassword;
}