using System.ComponentModel.DataAnnotations;

namespace UserService.WebApi.Register.Contracts;

public class RegisterContract
{
    public RegisterContract() { }

    public RegisterContract(
        string nickname,
        string email,
        string password,
        string confirmPassword,
        string? image,
        string secretWord)
    {
        Nickname = nickname;
        Email = email;
        Password = password;
        ConfirmPassword = confirmPassword;
        Image = image;
        SecretWord = secretWord;
    }

    [Required(ErrorMessage = "NickName is required")]
    [RegularExpression(
        @"^(?=.*[!_@])[a-zA-Z0-9!_@]{3,15}$",
        ErrorMessage =
            "NickName must be 3 to 15 characters long and " +
            "include at least one special character (!, _, @).")]
    public string Nickname { get; init; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(
        @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!_@])[a-zA-Z\d!_@]{5,80}$",
        ErrorMessage =
            "Password must be 5 to 80 characters long and include at least " +
            "one letter, one number, and one special character (!, _, @).")]
    public string Password { get; init; } = string.Empty;

    [Required(ErrorMessage = "Confirmation password is required")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; init; } = string.Empty;

    public string? Image { get; init; }

    [Required(ErrorMessage = "SecretWord is required")]
    [RegularExpression(
        @"^[a-zA-Z]{3,15}$",
        ErrorMessage =
            "SecretWord must be 3 to 15 characters long and contain only letters.")]
    public string SecretWord { get; init; } = string.Empty;
}