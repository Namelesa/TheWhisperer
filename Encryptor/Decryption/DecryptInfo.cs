using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Parameters;
using ChaCha20Poly1305 = Org.BouncyCastle.Crypto.Modes.ChaCha20Poly1305;

namespace Encryptor.Decryption;

public class DecryptInfo : IDecryptInfo
{
    private readonly byte[] _key;

    public DecryptInfo(IConfiguration configuration)
    {
        var key = configuration["Encryption:ChaChaKey"];
        if (string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException("Encryption key not configured");

        _key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
    }

    public string Decrypt(string cipherText)
    {
        var input = Convert.FromBase64String(cipherText);
        var nonce = input[..12];
        var ciphertextBytes = input[12..];

        var cipher = new ChaCha20Poly1305();
        var parameters = new AeadParameters(new KeyParameter(_key), 128, nonce, null);
        cipher.Init(false, parameters);

        var output = new byte[cipher.GetOutputSize(ciphertextBytes.Length)];
        var len = cipher.ProcessBytes(ciphertextBytes, 0, ciphertextBytes.Length, output, 0);
        cipher.DoFinal(output, len);

        return Encoding.UTF8.GetString(output);
    }

    public void DecryptObjectStrings<T>(T obj)
    {
        var excludedProps = new[]
        {
            "NickNameHash", "EmailHash", "SecretWortHash", "PasswordHash"
        };

        var props = typeof(T).GetProperties()
            .Where(p => 
                p is { CanRead: true, CanWrite: true } &&
                p.PropertyType == typeof(string) &&
                !excludedProps.Contains(p.Name));

        foreach (var prop in props)
        {
            var value = prop.GetValue(obj) as string;
            if (!string.IsNullOrEmpty(value))
            {
                prop.SetValue(obj, Decrypt(value));
            }
        }
    }
}