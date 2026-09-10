using System.ComponentModel.DataAnnotations;

namespace UserService.Core.EmailConfirmation;

public class UserEmailConfirmation(Guid userId)
{
    [Key]
    public Guid Id { get; init; }

    public Guid UserId { get; init; } = userId;
    
    public User.User User { get; init; }

    public string TokenHash { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime ExpiresAt { get; private set; }
    public DateTime? UsedAt { get; private set; }

    public void MarkAsUsed()
    {
        UsedAt = DateTime.UtcNow;
    }
    
    public void SetToken(string tokenHash)
    {
        TokenHash = tokenHash;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = CreatedAt.AddHours(24);
    }
}