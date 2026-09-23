namespace SharedModels.User.UserDelete;

public class UserDeleteByEmailModel(
    string nickName, 
    string email)
{
    public string NickName { get; init; } = nickName;
    public string Email { get; init; } = email;
}