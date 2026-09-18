namespace UserService.Core.RefreshToken;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshTokenModel token);
    Task<RefreshTokenModel?> GetByHashAsync(string tokenHash);
    Task UpdateAsync(RefreshTokenModel token);
    Task DeleteExpiredOrRevokedAsync();
    Task<List<RefreshTokenModel>> GetActiveByUserIdAsync(Guid userId);
}