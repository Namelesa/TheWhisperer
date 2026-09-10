using System.ComponentModel.DataAnnotations;

namespace UserService.WebApi.Login.Contracts;

public class LoginContract
{
    public LoginContract() { }
    
    public LoginContract(
        string nickName, 
        string password)
    {
        NickName = nickName;
        Password = password;
    }

    [Required(ErrorMessage = "NickName is required")]
    [RegularExpression(
        @"^(?=.*[!_@])[a-zA-Z0-9!_@]{3,15}$",
        ErrorMessage =
            "NickName must be 3 to 15 characters long and " +
            "include at least one special character (!, _, @).")]
    public string NickName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(
        @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!_@])[a-zA-Z\d!_@]{5,80}$",
        ErrorMessage =
            "Password must be 5 to 80 characters long and include at least " +
            "one letter, one number, and one special character (!, _, @).")]
    public string Password { get; init; } = string.Empty;
}