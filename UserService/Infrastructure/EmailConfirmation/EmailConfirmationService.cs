using System.Security.Cryptography;
using System.Text;
using UserService.Application.EmailConfirmation;

namespace UserService.Infrastructure.EmailConfirmation;

public class EmailConfirmationService : IEmailConfirmationService
{
    public string GenerateToken()
    {
        var token = RandomNumberGenerator.GetBytes(32);

        return Convert.ToBase64String(token);
    }

    public string HashToken(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = SHA256.HashData(bytes);

        return Convert.ToBase64String(hash);
    }
}