using Microsoft.EntityFrameworkCore;
using UserService.Core.ForgotPassword;

namespace UserService.Persistence.UserForgotPassword;

public class UserForgotPasswordRepository(AppDbContext db) : IUserForgotPasswordRepository
{
    public async Task AddAsync(Core.ForgotPassword.UserForgotPassword userForgotPassword)
    {
        await db.UserForgotPasswords.AddAsync(userForgotPassword);
        await db.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Core.ForgotPassword.UserForgotPassword userForgotPassword)
    {
        db.UserForgotPasswords.Update(userForgotPassword);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAllByUserIdAsync(Guid userId)
    {
        await db.UserForgotPasswords
            .Where(x => x.UserId == userId)
            .ExecuteDeleteAsync();
    }
    
    public async Task<Core.ForgotPassword.UserForgotPassword?> GetByTokenHashAsync(string tokenHash) =>
        await db.UserForgotPasswords
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);
}