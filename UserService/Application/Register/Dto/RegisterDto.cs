namespace UserService.Application.Register.Dto;

public class RegisterDto(
    string nickName, 
    string email, 
    string password, 
    string? image, 
    string secretWord)
{
    public string NickName { get; init; } = nickName;
    public string Email { get; init; } = email;
    public string Password { get; init; } = password;
    public string? Image { get; init; } = image;
    public string SecretWord { get; init; } = secretWord;
}