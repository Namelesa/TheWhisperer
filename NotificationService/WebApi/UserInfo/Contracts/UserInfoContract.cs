using System.ComponentModel.DataAnnotations;

namespace NotificationService.WebApi.UserInfo.Contracts;

public class UserInfoContract
{
    public UserInfoContract() { }
    
    public UserInfoContract(
        string? nickName,
        string? email,
        string token)
    {
        NickName = nickName;
        Email = email;
        Token = token;
    }
    
    [RegularExpression(
        @"^(?=.*[!_@])[a-zA-Z0-9!_@]{3,15}$",
        ErrorMessage =
            "NickName must be 3 to 15 characters long and " +
            "include at least one special character (!, _, @).")]
    public string? NickName { get; init; } = string.Empty;
    
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string? Email { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
}