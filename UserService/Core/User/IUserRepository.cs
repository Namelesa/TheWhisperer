namespace UserService.Core.User;

public interface IUserRepository
{
    public Task AddUserAsync(User user);
    public Task <User?> GetUserByNickNameHashAsync(string nickNameHash);
    public Task <User?> GetUserByEmailHashAsync(string emailHash);
    public Task DeleteUserAsync(User user);
    public Task UpdateUserAsync(User user);
    public Task DeleteUnconfirmedUsersAsync();
}