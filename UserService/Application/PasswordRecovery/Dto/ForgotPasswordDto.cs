namespace UserService.Application.PasswordRecovery.Dto;

public class ForgotPasswordDto(string email)
{
    public string Email { get; init; } = email;
}