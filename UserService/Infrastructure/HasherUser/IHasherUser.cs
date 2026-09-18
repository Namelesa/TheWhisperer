namespace UserService.Infrastructure.HasherUser;

public interface IHasherUser
{
    public string Hash(string input);
    public bool Verify(
        string login, 
        string storedHashBase64);
}