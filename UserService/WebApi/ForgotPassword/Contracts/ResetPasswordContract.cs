using System.ComponentModel.DataAnnotations;

namespace UserService.WebApi.ForgotPassword.Contracts;

public class ResetPasswordContract
{
    public ResetPasswordContract() { }

    public ResetPasswordContract(
        string secretWort, 
        string token,
        string newPassword,
        string newPasswordConfirm)
    {
        SecretWort = secretWort;
        Token = token;
        NewPassword = newPassword;
        NewPasswordConfirm = newPasswordConfirm;
    }
    
    [Required(ErrorMessage = "SecretWord is required")]
    [RegularExpression(
        @"^[a-zA-Z]{3,15}$",
        ErrorMessage =
            "SecretWord must be 3 to 15 characters long and contain only letters.")]
    public string SecretWort { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(
        @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!_@])[a-zA-Z\d!_@]{5,80}$",
        ErrorMessage =
            "Password must be 5 to 80 characters long and include at least " +
            "one letter, one number, and one special character (!, _, @).")]
    public string NewPassword { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Confirmation password is required")]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
    public string NewPasswordConfirm { get; init; } = string.Empty;
}