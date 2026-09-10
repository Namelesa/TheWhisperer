using FluentValidation;
using UserService.Application.User.Dto;

namespace UserService.Application.User;

public class UserValidator : AbstractValidator<EditUserDto>
{
    public UserValidator()
    {
        RuleFor(u => u.NewNickName)
            .Matches(@"^(?=.*[!_@])[a-zA-Z0-9!_@]{3,15}$")
            .WithMessage(
                "NickName must be 3 to 15 characters long and " +
                "include at least one special character (!, _, @).");

        RuleFor(u => u.Email)
            .EmailAddress()
            .WithMessage("Invalid email.");
        
        RuleFor(u => u.Image)
            .MaximumLength(500)
            .WithMessage("Image must not exceed 500 characters.")
            .When(u => !string.IsNullOrEmpty(u.Image));
    }
}