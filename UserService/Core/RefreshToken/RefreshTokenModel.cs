namespace UserService.Core.RefreshToken;

public class RefreshTokenModel
{
    public Guid Id { get; private init; }
    public Guid UserId { get; private init; }
    public User.User User { get; init; }
    public string TokenHash { get; private init; }
    public DateTime CreatedAt { get; private init; }
    public DateTime ExpiresAt { get; private init; }
    public DateTime? RevokedAt { get; private set; }

    public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;

    private RefreshTokenModel() { }

    public static RefreshTokenModel Create(Guid userId, string tokenHash, TimeSpan lifetime)
    {
        var now = DateTime.UtcNow;
        return new RefreshTokenModel
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = tokenHash,
            CreatedAt = now,
            ExpiresAt = now.Add(lifetime)
        };
    }

    public void Revoke()
    {
        RevokedAt ??= DateTime.UtcNow;
    }
}