namespace Encryptor.Encryption;

public interface IEncryptInfo
{
    string Encrypt(string plainText);
    public void EncryptObjectStrings<T>(T obj);
}