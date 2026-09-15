using System.ComponentModel.DataAnnotations;

namespace UserService.Core.User;

public class User(
    string nickName, 
    string email, 
    string? image)
{
    [Key]
    public Guid Id { get; init; }
    public string NickName { get; private set; } = nickName;
    public string Email { get; private set; } = email;
    public string EmailHash { get; private set; } 
    public bool ConfirmedEmail { get; private set; }
    public string? Image { get; private set; } = image;
    public string NickNameHash { get; private set; }
    public string SecretWortHash { get; private set; }
    public string PasswordHash { get; private set; }

    public void SetHashes(string nickNameHash, string secretWortHash, string emailHash, string passwordHash)
    {
        NickNameHash = nickNameHash;
        SecretWortHash = secretWortHash;
        EmailHash = emailHash;
        PasswordHash = passwordHash;
    }
    
    public void SetImage(string? image)
    {
        Image = image;
    }
    
    public void ConfirmEmail()
    {
        ConfirmedEmail = true;
    }
    
    public void UpdateNickName(string newNickName, string newNickNameHash)
    {
        NickName = newNickName;
        NickNameHash = newNickNameHash;
    }
    
    public void UpdateEmail(string newEmail, string newEmailHash)
    {
        Email = newEmail;
        EmailHash = newEmailHash;
        ConfirmedEmail = false;
    }
    
    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }
}