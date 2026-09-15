using FluentValidation;
using UserService.Application.HasherPassword;
using UserService.Application.HasherUser;
using UserService.Application.Login.Dto;
using UserService.Core.User;

namespace UserService.Application.Login;

public class LoginOrchestrator(
    IUserRepository userRepository, 
    IHasherUser hasherUser, 
    IHasherPassword hasherPassword,
    IValidator<LoginDto> validator) : ILoginOrchestrator
{
    public async Task<OperationResult<string>> LoginAsync(LoginDto loginDto)
    {
        var validationResult = await validator.ValidateAsync(loginDto);
        if (!validationResult.IsValid) 
            return OperationResult<string>.Fail(
                string.Join("; ", validationResult.Errors));
        
        var hashedNickName = hasherUser.Hash(loginDto.NickName);
        var user = await userRepository.GetUserByNickNameHashAsync(hashedNickName);
        
        if (user is null)
            return OperationResult<string>.Fail("User with this nick name does not exist");
        
        if(!user.ConfirmedEmail)
            return OperationResult<string>.Fail("Email is not confirmed");
        
        return !hasherPassword.Verify(loginDto.Password, user.PasswordHash) 
            ? OperationResult<string>.Fail("Incorrect password") 
            : OperationResult<string>.Ok("User logged in successfully");
    }
}