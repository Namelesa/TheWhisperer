namespace NotificationService.Core.UserDelete;

public class UserDeleteModel
{
    public UserDeleteModel() { }
    
    public UserDeleteModel(string nickName, string email)
    {
        NickName = nickName;
        Email = email;
    }
    public string NickName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}