namespace UserService.Application.Login.Dto;

public class LoginDto(
    string nickName, 
    string password)
{
    public string NickName { get; init; } = nickName;
    public string Password { get; init; } = password;
}