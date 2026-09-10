using System.ComponentModel.DataAnnotations;

namespace UserService.WebApi.User.Contracts;

public class EditUserContract
{
    
    public EditUserContract() { }
    
    public EditUserContract(
        string? newNickName,
        string? email,
        string? image)
    {
        NewNickName = newNickName;
        Email = email;
        Image = image;
    }
    
    [RegularExpression(
        @"^(?=.*[!_@])[a-zA-Z0-9!_@]{3,15}$",
        ErrorMessage =
            "NickName must be 3 to 15 characters long and " +
            "include at least one special character (!, _, @).")]
    public string? NewNickName { get; init; }
    
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string? Email { get; init; } 
    
    public string? Image { get; init; }
}