using System.ComponentModel.DataAnnotations;

namespace UserService.WebApi.ForgotPassword.Contracts;

public class ForgotPasswordContract
{
    public ForgotPasswordContract() { }

    public ForgotPasswordContract(string email)
    {
        Email = email;
    }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; init; } = string.Empty;
}