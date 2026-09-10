using Microsoft.EntityFrameworkCore;
using UserService.Core.EmailConfirmation;

namespace UserService.Persistence.UserEmailConfirmation;

public class UserEmailConfirmationRepository(AppDbContext db) : IUserEmailConfirmationRepository
{
    public async Task AddAsync(Core.EmailConfirmation.UserEmailConfirmation token)
    {
        await db.UserEmailConfirmations.AddAsync(token);
        await db.SaveChangesAsync();
    }

    public async Task<Core.EmailConfirmation.UserEmailConfirmation?> GetByTokenHashAsync(string tokenHash) => 
        await db.UserEmailConfirmations
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);

    public async Task UpdateAsync(Core.EmailConfirmation.UserEmailConfirmation token)
    {
        db.UserEmailConfirmations.Update(token);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Core.EmailConfirmation.UserEmailConfirmation token)
    {
        db.UserEmailConfirmations.Remove(token);
        await db.SaveChangesAsync();
    }
    
    public async Task DeleteByUserIdAsync(Guid userId)
    {
        var confirmations = await db.UserEmailConfirmations
            .Where(x => x.UserId == userId)
            .ToListAsync();

        if (confirmations.Count == 0)
            return;

        db.UserEmailConfirmations.RemoveRange(confirmations);
        await db.SaveChangesAsync();
    }
}