namespace Encryptor.Decryption;

public interface IDecryptInfo
{
    string Decrypt(string cipherText);
    void DecryptObjectStrings<T>(T obj);
}