namespace NotificationService.Core.UserInfo;

public class UserInfoModel
{
    public UserInfoModel() { }
    
    public UserInfoModel(
        string nickName,
        string email,
        string? token)
    {
        NickName = nickName;
        Email = email;
        Token = token;
    }

    public string NickName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Token { get; init; } = string.Empty;
    
    public void SetNickNameAndEmail(string nickName, string email)
    {
        NickName = nickName;
        Email = email;
    }
}