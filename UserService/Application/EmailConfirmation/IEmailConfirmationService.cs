namespace UserService.Application.EmailConfirmation;

public interface IEmailConfirmationService
{
    public string GenerateToken();
    public string HashToken(string token);
}