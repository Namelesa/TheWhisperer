namespace NotificationService.WebApi.UserDelete.Contracts;

public class UserDeleteContract
{
    public UserDeleteContract() { }
    
    public UserDeleteContract(
        string nickName,
        string email)
    {
        NickName = nickName;
        Email = email;
    }

    public string NickName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}