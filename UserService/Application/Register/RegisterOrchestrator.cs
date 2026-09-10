using AutoMapper;
using FluentValidation;
using UserService.Application.EmailConfirmation;
using UserService.Application.HasherPassword;
using UserService.Application.HasherUser;
using UserService.Application.Register.Dto;
using UserService.Core.EmailConfirmation;
using UserService.Core.User;

namespace UserService.Application.Register;

public class RegisterOrchestrator(
    IUserRepository userRepository, 
    IMapper mapper,
    IHasherUser hasherUser,
    IHasherPassword hasherPassword,
    IValidator<RegisterDto> validator,
    IEmailConfirmationService emailConfirmationService,
    IUserEmailConfirmationRepository userEmailConfirmationRepository) : IRegisterOrchestrator
{
    public async Task<OperationResult<string>> RegisterUserAsync(RegisterDto registerDto)
    {
        var validationResult = await validator.ValidateAsync(registerDto);
        if (!validationResult.IsValid) 
            return OperationResult<string>.Fail(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
        
        var hashedNickName = hasherUser.Hash(registerDto.NickName);
        var hashedEmail = hasherUser.Hash(registerDto.Email);

        if (await userRepository.GetUserByNickNameHashAsync(hashedNickName) is not null || 
            await userRepository.GetUserByEmailHashAsync(hashedEmail) is not null)
            return OperationResult<string>.Fail("User with this nick name or email already exists");
        
        var hashedSecretWord = hasherUser.Hash(registerDto.SecretWord);
        var hashedPassword = hasherPassword.Hash(registerDto.Password);
        
        var user = mapper.Map<Core.User.User>(registerDto);
        user.SetHashes(hashedNickName, 
            hashedSecretWord, 
            hashedEmail, 
            hashedPassword);
        
        //encrypt

        await userRepository.AddUserAsync(user);
        
        var token = emailConfirmationService.GenerateToken();
        var tokenHash = emailConfirmationService.HashToken(token);

        var confirmation = new UserEmailConfirmation(user.Id);
        confirmation.SetToken(tokenHash);

        // Send email with the token to the user's email address
        
        await userEmailConfirmationRepository.AddAsync(confirmation);
        
        return OperationResult<string>.Ok("Registration successful. Please check your email.");
    }

    public async Task<OperationResult<string>> EmailConfirmationAsync(
        string emailConfirmationToken,
        string userNickName)
    {
        var hashedNickName = hasherUser.Hash(userNickName);

        var user = await userRepository
            .GetUserByNickNameHashAsync(hashedNickName);

        if (user is null)
            return OperationResult<string>.Fail("User not found");

        if (user.ConfirmedEmail)
            return OperationResult<string>.Fail("Email is already confirmed");

        var confirmation = await userEmailConfirmationRepository
            .GetByTokenHashAsync(
                emailConfirmationService.HashToken(emailConfirmationToken));

        if (confirmation is null)
            return OperationResult<string>.Fail("Invalid confirmation token");

        if (confirmation.UserId != user.Id)
            return OperationResult<string>.Fail("Invalid confirmation token");

        if (confirmation.UsedAt is not null)
            return OperationResult<string>.Fail("Confirmation token has already been used");
        
        if (confirmation.ExpiresAt <= DateTime.UtcNow)
        {
            await userEmailConfirmationRepository.DeleteAsync(
                confirmation);

            return OperationResult<string>.Fail("Confirmation token has expired");
        }

        user.ConfirmEmail();
        confirmation.MarkAsUsed();
        
        await userRepository.UpdateUserAsync(user);
        await userEmailConfirmationRepository.UpdateAsync(confirmation);
        
        return OperationResult<string>.Ok("Email confirmed successfully");
    }
}