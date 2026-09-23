namespace SharedModels.User.UserConfirmEmail;

public class UserEmailConfirmModel(
    string nickName, 
    string email, 
    string token)
{
    public string NickName { get; set; } = nickName;
    public string Email { get; set; } = email;
    public string Token { get; set; } = token;
}