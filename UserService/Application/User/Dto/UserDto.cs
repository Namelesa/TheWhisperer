namespace UserService.Application.User.Dto;

public class UserDto(
    string nickName, 
    string image, 
    string email)
{
    public string NickName { get; init; } = nickName;
    public string Image { get; init; } = image;
    public string Email { get; init; } = email;
}