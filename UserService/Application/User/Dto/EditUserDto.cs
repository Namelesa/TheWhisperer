namespace UserService.Application.User.Dto;

public class EditUserDto(
    string? newNickName, 
    string? email, 
    string? image)
{
    public string? NewNickName { get; private set; } = newNickName;
    public string? Email { get; private set; } = email;
    public string? Image { get; private set; } = image;
}