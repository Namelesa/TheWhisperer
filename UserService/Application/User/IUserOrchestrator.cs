using UserService.Application.User.Dto;

namespace UserService.Application.User;

public interface IUserOrchestrator
{
    public Task<OperationResult<string>> UpdateUserAsync(
        EditUserDto editUserDto, 
        string nickName);
    public Task<OperationResult<string>> DeleteUserAsync(string nickName);
    public Task<OperationResult<string>> UpdateUserPasswordAsync(EditUserPasswordDto editUserPasswordDto);
}