using System.ComponentModel.DataAnnotations;

namespace UserService.WebApi.User.Contracts;

public class EditUserPasswordContract
{
    public EditUserPasswordContract() { }
    
    public EditUserPasswordContract(
        string nickName,
        string newPassword,
        string secretWord)
    {
        NickName = nickName;
        NewPassword = newPassword;
        SecretWord = secretWord;
    }
    
    [Required(ErrorMessage = "NickName is required")]
    [RegularExpression(
        @"^(?=.*[!_@])[a-zA-Z0-9!_@]{3,15}$",
        ErrorMessage =
            "NickName must be 3 to 15 characters long and " +
            "include at least one special character (!, _, @).")]
    public string NickName { get; init; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(
        @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!_@])[a-zA-Z\d!_@]{5,80}$",
        ErrorMessage =
            "Password must be 5 to 80 characters long and include at least " +
            "one letter, one number, and one special character (!, _, @).")]
    public string NewPassword { get; init; }

    [Required(ErrorMessage = "SecretWord is required")]
    [RegularExpression(
        @"^[a-zA-Z]{3,15}$",
        ErrorMessage =
            "SecretWord must be 3 to 15 characters long and contain only letters.")]
    public string SecretWord { get; init; }
}