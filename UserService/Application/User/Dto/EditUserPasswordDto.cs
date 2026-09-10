namespace UserService.Application.User.Dto;

public class EditUserPasswordDto(
    string nickName, 
    string newPassword, 
    string secretWord)
{
    public string NickName { get; init; } = nickName;
    public string NewPassword { get; init; } = newPassword;
    public string SecretWord { get; init; } = secretWord;
}