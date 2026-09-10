using FluentValidation;
using UserService.Application.Register.Dto;

namespace UserService.Application.Register;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(u => u.NickName)
            .NotEmpty()
            .WithMessage("NickName is required.")
            .Matches(@"^(?=.*[!_@])[a-zA-Z0-9!_@]{3,15}$")
            .WithMessage(
                "NickName must be 3 to 15 characters long and " +
                "include at least one special character (!, _, @).");

        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email.");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .Matches(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!_@])[a-zA-Z\d!_@]{5,80}$")
            .WithMessage(
                "Password must be 5 to 80 characters long and include at least " +
                "one letter, one number, and one special character (!, _, @).");

        RuleFor(u => u.SecretWord)
            .NotEmpty()
            .WithMessage("SecretWord is required.")
            .Matches(@"^[a-zA-Z]{3,15}$")
            .WithMessage(
                "SecretWord must be 3 to 15 characters long and contain only letters.");

        RuleFor(u => u.Image)
            .MaximumLength(500)
            .WithMessage("Image must not exceed 500 characters.")
            .When(u => !string.IsNullOrEmpty(u.Image));
    }
}