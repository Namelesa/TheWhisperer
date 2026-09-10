using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using UserService.Application.HasherPassword;

namespace UserService.Infrastructure.HasherPassword;

public class HasherPassword : IHasherPassword
{
    private const int SaltSize = 16;
    private const int HashSize = 32;

    private const int DegreeOfParallelism = 4;
    private const int Iterations = 4;
    private const int MemorySizeKb = 65536;

    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException(
                "Password must not be null or empty.",
                nameof(password));
        }

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = ComputeHash(password, salt);

        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool Verify(
        string password, 
        string storedHash)
    {
        if (string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(storedHash))
        {
            return false;
        }

        var parts = storedHash.Split('.', 2);

        if (parts.Length != 2)
            return false;

        try
        {
            var salt = Convert.FromBase64String(parts[0]);
            var expectedHash = Convert.FromBase64String(parts[1]);

            if (salt.Length != SaltSize ||
                expectedHash.Length != HashSize)
            {
                return false;
            }

            var actualHash = ComputeHash(password, salt);

            return CryptographicOperations.FixedTimeEquals(
                actualHash,
                expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static byte[] ComputeHash(
        string password,
        byte[] salt)
    {
        using var argon2 = new Argon2id(
            Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            Iterations = Iterations,
            MemorySize = MemorySizeKb
        };

        return argon2.GetBytes(HashSize);
    }
}