using Microsoft.EntityFrameworkCore;
using UserService.Core.RefreshToken;

namespace UserService.Persistence.RefreshToken;

public class RefreshTokenRepository(AppDbContext db) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshTokenModel token)
    {
        await db.RefreshTokens.AddAsync(token);
        await db.SaveChangesAsync();
    }

    public async Task<RefreshTokenModel?> GetByHashAsync(string tokenHash) =>
        await db.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

    public async Task UpdateAsync(RefreshTokenModel token)
    {
        db.RefreshTokens.Update(token);
        await db.SaveChangesAsync();
    }

    public async Task DeleteExpiredOrRevokedAsync()
    {
        var now = DateTime.UtcNow;

        await db.RefreshTokens
            .Where(x => x.ExpiresAt < now || x.RevokedAt != null)
            .ExecuteDeleteAsync();
    }
    
    public async Task<List<RefreshTokenModel>> GetActiveByUserIdAsync(Guid userId)
    {
        var now = DateTime.UtcNow;

        return await db.RefreshTokens
            .Where(x => x.UserId == userId && x.RevokedAt == null && x.ExpiresAt > now)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
    }
}