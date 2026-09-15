namespace UserService.Infrastructure.ForgotPassword;

public interface IUserForgotPasswordService
{
    public string GenerateToken();
    public string HashToken(string token);
}