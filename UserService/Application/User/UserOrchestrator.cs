using FluentValidation;
using UserService.Application.EmailConfirmation;
using UserService.Application.HasherPassword;
using UserService.Application.HasherUser;
using UserService.Application.User.Dto;
using UserService.Core.EmailConfirmation;
using UserService.Core.User;

namespace UserService.Application.User;

public class UserOrchestrator(
    IUserRepository userRepository,
    IValidator<EditUserDto> validator,
    IHasherUser hasherUser,
    IHasherPassword hasherPassword,
    IUserEmailConfirmationRepository userEmailConfirmationRepository,
    IEmailConfirmationService emailConfirmationService) : IUserOrchestrator
{
    public async Task<OperationResult<string>> UpdateUserAsync(
        EditUserDto editUserDto,
        string nickName)
    {
        var validationResult = await validator.ValidateAsync(editUserDto);
        if (!validationResult.IsValid) 
            return OperationResult<string>.Fail(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
        
        if (editUserDto.NewNickName is null &&
            editUserDto.Email is null &&
            editUserDto.Image is null)
            return OperationResult<string>.Fail("No data to update");

        var hashedNickName = hasherUser.Hash(nickName);
        var user = await userRepository.GetUserByNickNameHashAsync(hashedNickName);

        if (user is null)
            return OperationResult<string>.Fail("User with this nick name does not exist");

        if (editUserDto.NewNickName is not null)
        {
            var newNickNameHash = hasherUser.Hash(editUserDto.NewNickName);
            var existingUser = await userRepository.GetUserByNickNameHashAsync(newNickNameHash);

            if (existingUser is not null && existingUser.Id != user.Id)
                return OperationResult<string>.Fail("User with this nick name already exists");

            user.UpdateNickName(editUserDto.NewNickName, newNickNameHash);
        }

        if (editUserDto.Email is not null)
        {
            var newEmailHash = hasherUser.Hash(editUserDto.Email);

            var existingUser = await userRepository.GetUserByEmailHashAsync(newEmailHash);

            if (existingUser is not null && existingUser.Id != user.Id)
                return OperationResult<string>.Fail("User with this email already exists");

            user.UpdateEmail(
                editUserDto.Email,
                newEmailHash);

            await userEmailConfirmationRepository
                .DeleteByUserIdAsync(user.Id);
            
            var token = emailConfirmationService.GenerateToken();
            var tokenHash = emailConfirmationService.HashToken(token);

            var confirmation = new UserEmailConfirmation(user.Id);
            confirmation.SetToken(tokenHash);

            await userEmailConfirmationRepository.AddAsync(confirmation);
        }

        if (editUserDto.Image is not null)
        {
            user.SetImage(editUserDto.Image);
        }

        await userRepository.UpdateUserAsync(user);
        return OperationResult<string>.Ok("User data updated successfully");
    }

    public async Task<OperationResult<string>> DeleteUserAsync(string nickName)
    {
        var hashedNickName = hasherUser.Hash(nickName);
        var user = await userRepository.GetUserByNickNameHashAsync(hashedNickName);

        if (user is null)
            return OperationResult<string>.Fail("User with this nick name does not exist");

        await userRepository.DeleteUserAsync(user);

        return OperationResult<string>.Ok("User deleted successfully");
    }

    public async Task<OperationResult<string>> UpdateUserPasswordAsync(EditUserPasswordDto editUserPasswordDto)
    {
        var hashedNickName = hasherUser.Hash(editUserPasswordDto.NickName);
        var user = await userRepository.GetUserByNickNameHashAsync(hashedNickName);

        if (user is null)
            return OperationResult<string>.Fail("User with this nick name does not exist");
        
        if (!hasherUser.Verify(editUserPasswordDto.SecretWord, user.SecretWortHash))
            return OperationResult<string>.Fail("Incorrect secret word");
        
        var newPasswordHash = hasherPassword.Hash(editUserPasswordDto.NewPassword);
        user.UpdatePassword(newPasswordHash);
        
        await userRepository.UpdateUserAsync(user);
        return OperationResult<string>.Ok("User data updated successfully");
    }
}