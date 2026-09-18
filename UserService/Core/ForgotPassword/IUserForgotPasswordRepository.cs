namespace UserService.Core.ForgotPassword;

public interface IUserForgotPasswordRepository
{
    public Task AddAsync(UserForgotPassword userForgotPassword);
    
    public Task UpdateAsync(UserForgotPassword userForgotPassword);

    public Task DeleteAllByUserIdAsync(Guid userId);
    
    public Task<UserForgotPassword?> GetByTokenHashAsync(string tokenHash);
}