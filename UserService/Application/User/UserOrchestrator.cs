using AutoMapper;
using FluentValidation;
using Encryptor.Decryption;
using Encryptor.Encryption;
using UserService.Application.HasherUser;
using UserService.Application.User.Dto;
using UserService.Core.EmailConfirmation;
using UserService.Core.User;
using UserService.Infrastructure.EmailConfirmation;

namespace UserService.Application.User;

public class UserOrchestrator(
    IUserRepository userRepository,
    IValidator<EditUserDto> validator,
    IHasherUser hasherUser,
    IMapper mapper,
    IUserEmailConfirmationRepository userEmailConfirmationRepository,
    IEmailConfirmationService emailConfirmationService,
    IDecryptInfo decryptInfo,
    IEncryptInfo encryptInfo) : IUserOrchestrator
{
    public async Task<OperationResult<UserDto>> GetUserByIdAsync(Guid userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user is null)
            return OperationResult<UserDto>.Fail("User with this nick name does not exist");

        decryptInfo.DecryptObjectStrings(user);
        var userDto = mapper.Map<UserDto>(user);
        
        return OperationResult<UserDto>.Ok(userDto);
    }
    
    public async Task<OperationResult<string>> UpdateUserAsync(
        EditUserDto editUserDto,
        Guid userId)
    {
        var validationResult = await validator.ValidateAsync(editUserDto);
        if (!validationResult.IsValid) 
            return OperationResult<string>.Fail(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
        
        if (editUserDto.NewNickName is null &&
            editUserDto.Email is null &&
            editUserDto.Image is null)
            return OperationResult<string>.Fail("No data to update");
        
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user is null)
            return OperationResult<string>.Fail("User with this nick name does not exist");

        if (editUserDto.NewNickName is not null)
        {
            var newNickNameHash = hasherUser.Hash(editUserDto.NewNickName);
            var existingUser = await userRepository.GetUserByIdAsync(userId);

            if (existingUser is not null && existingUser.Id != user.Id)
                return OperationResult<string>.Fail("User with this nick name already exists");

            var encryptedNewNickName = encryptInfo.Encrypt(editUserDto.NewNickName);
            user.UpdateNickName(encryptedNewNickName, newNickNameHash);
        }

        if (editUserDto.Email is not null)
        {
            var newEmailHash = hasherUser.Hash(editUserDto.Email);
            var existingUser = await userRepository.GetUserByEmailHashAsync(newEmailHash);

            if (existingUser is not null && existingUser.Id != user.Id)
                return OperationResult<string>.Fail("User with this email already exists");
            
            var encryptedEmail = encryptInfo.Encrypt(editUserDto.Email);
            user.UpdateEmail(encryptedEmail, newEmailHash);

            await userEmailConfirmationRepository
                .DeleteAllByUserIdAsync(user.Id);
            
            var token = emailConfirmationService.GenerateToken();
            var tokenHash = emailConfirmationService.HashToken(token);

            var confirmation = new UserEmailConfirmation(user.Id);
            confirmation.SetToken(tokenHash);

            // Send email confirmation to the new email address
            
            await userEmailConfirmationRepository.AddAsync(confirmation);
        }

        if (editUserDto.Image is not null)
        {
            var encryptedImage = encryptInfo.Encrypt(editUserDto.Image);
            user.SetImage(encryptedImage);
        }

        await userRepository.UpdateUserAsync(user);
        return OperationResult<string>.Ok("User data updated successfully");
    }

    public async Task<OperationResult<string>> DeleteUserAsync(Guid userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user is null)
            return OperationResult<string>.Fail("User with this nick name does not exist");

        await userRepository.DeleteUserAsync(user);

        return OperationResult<string>.Ok("User deleted successfully");
    }
}