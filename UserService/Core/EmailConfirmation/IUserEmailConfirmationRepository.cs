namespace UserService.Core.EmailConfirmation;

public interface IUserEmailConfirmationRepository
{
    public Task AddAsync(UserEmailConfirmation token);

    public Task<UserEmailConfirmation?> GetByTokenHashAsync(
        string tokenHash);

    public Task UpdateAsync(UserEmailConfirmation token);

    public Task DeleteAsync(UserEmailConfirmation token);
    
    public Task DeleteByUserIdAsync(Guid userId);
}