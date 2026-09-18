namespace UserService.Infrastructure.HasherPassword;

public interface IHasherPassword
{
    string Hash(string password);
    bool Verify(
        string password, 
        string storedHash);
}