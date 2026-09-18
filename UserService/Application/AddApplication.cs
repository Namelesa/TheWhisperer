using FluentValidation;
using UserService.Application.Login;
using UserService.Application.Login.Dto;
using UserService.Application.Logout;
using UserService.Application.PasswordRecovery;
using UserService.Application.RefreshToken;
using UserService.Application.Register;
using UserService.Application.Register.Dto;
using UserService.Application.User;
using UserService.Application.User.Dto;

namespace UserService.Application;

public static class AddApplication
{
    public static void AddApplicationLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IRegisterOrchestrator, RegisterOrchestrator>();
        services.AddScoped<ILoginOrchestrator, LoginOrchestrator>();
        services.AddScoped<IUserOrchestrator, UserOrchestrator>();
        services.AddScoped<IPasswordRecoveryOrchestrator, PasswordRecoveryOrchestrator>();
        services.AddScoped<IRefreshTokenOrchestrator, RefreshTokenOrchestrator>();
        services.AddScoped<ILogoutOrchestrator, LogoutOrchestrator>();
        services.AddScoped<IValidator<RegisterDto>, RegisterValidator>();
        services.AddScoped<IValidator<LoginDto>, LoginValidator>(); 
        services.AddScoped<IValidator<EditUserDto>, UserValidator>(); 
        
        services.AddOptions<RefreshTokenPolicySettings>()
            .Bind(configuration.GetSection("RefreshTokenPolicy"))
            .Validate(s => s.MaxActiveRefreshTokensPerUser > 0,
                "RefreshTokenPolicy:MaxActiveRefreshTokensPerUser must be greater than 0")
            .ValidateOnStart();
    }
}