using UserService.Application.User.Dto;

namespace UserService.Application.User;

public interface IUserOrchestrator
{
    public Task<OperationResult<string>> UpdateUserAsync(
        EditUserDto editUserDto, 
        Guid userId);
    public Task<OperationResult<string>> DeleteUserAsync(Guid userId);
    public Task<OperationResult<UserDto>> GetUserByIdAsync(Guid userId);
}