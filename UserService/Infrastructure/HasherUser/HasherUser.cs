using System.Security.Cryptography;
using System.Text;

namespace UserService.Infrastructure.HasherUser;

public class HasherUser(byte[] secretKey) : IHasherUser
{
    private readonly byte[] _secretKey = secretKey.Length >= 32
        ? secretKey
        : throw new ArgumentException(
            "The key must be at least 32 bytes long.",
            nameof(secretKey));

    public string Hash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "The value must not be null or empty.",
                nameof(value));

        var normalizedValue = Normalize(value);
        var data = Encoding.UTF8.GetBytes(normalizedValue);

        using var hmac = new HMACSHA256(_secretKey);

        var hash = hmac.ComputeHash(data);

        return Convert.ToBase64String(hash);
    }

    public bool Verify(
        string value, 
        string storedHash)
    {
        if (string.IsNullOrWhiteSpace(value) ||
            string.IsNullOrWhiteSpace(storedHash))
            return false;

        byte[] storedHashBytes;

        try
        {
            storedHashBytes = Convert.FromBase64String(storedHash);
        }
        catch (FormatException)
        {
            return false;
        }

        var computedHash = Hash(value);
        var computedHashBytes = Convert.FromBase64String(computedHash);

        return CryptographicOperations.FixedTimeEquals(
            computedHashBytes,
            storedHashBytes);
    }

    private static string Normalize(string value)
    {
        return value.Trim().ToLowerInvariant();
    }

    public static HasherUser FromBase64Key(string base64Key)
    {
        if (string.IsNullOrWhiteSpace(base64Key))
            throw new ArgumentException(
                "The key must not be null or empty.",
                nameof(base64Key));

        var key = Convert.FromBase64String(base64Key);

        return new HasherUser(key);
    }

    public static string GenerateBase64Key(int lengthBytes = 32)
    {
        if (lengthBytes < 32)
            throw new ArgumentException(
                "The key must be at least 32 bytes long.",
                nameof(lengthBytes));

        var key = RandomNumberGenerator.GetBytes(lengthBytes);

        return Convert.ToBase64String(key);
    }
}