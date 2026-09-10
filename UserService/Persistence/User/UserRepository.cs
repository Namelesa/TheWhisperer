using Microsoft.EntityFrameworkCore;
using UserService.Core.User;

namespace UserService.Persistence.User;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public async Task AddUserAsync(Core.User.User user)
    {
        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();
    }

    public async Task<Core.User.User?> GetUserByNickNameHashAsync(string nickNameHash) =>
        await db.Users.FirstOrDefaultAsync(u => u.NickNameHash == nickNameHash);

    public async Task<Core.User.User?> GetUserByEmailHashAsync(string emailHash) =>
        await db.Users.FirstOrDefaultAsync(u => u.EmailHash == emailHash);
    
    public async Task DeleteUserAsync(Core.User.User user)
    {
        db.Users.Remove(user);
        await db.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(Core.User.User user)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync();
    }
    
    public async Task DeleteUnconfirmedUsersAsync()
    {
        var users = await db.Users
            .Where(user =>
                !user.ConfirmedEmail &&
                db.UserEmailConfirmations.Any(token =>
                    token.UserId == user.Id &&
                    token.ExpiresAt <= DateTime.UtcNow &&
                    token.UsedAt == null))
            .ToListAsync();

        if (users.Count == 0)
            return;

        db.Users.RemoveRange(users);
        await db.SaveChangesAsync();
    }
}