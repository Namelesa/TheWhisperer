using System.Security.Cryptography;
using System.Text;

namespace UserService.Infrastructure.ForgotPassword;

public class UserForgotPasswordService : IUserForgotPasswordService
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