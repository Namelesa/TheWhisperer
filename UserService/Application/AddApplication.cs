using UserService.Application.Login;
using UserService.Application.Login.Dto;
using UserService.Application.Register;
using UserService.Application.Register.Dto;
using UserService.Application.User;
using FluentValidation;
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
        services.AddScoped<IValidator<RegisterDto>, RegisterValidator>();
        services.AddScoped<IValidator<LoginDto>, LoginValidator>(); 
        services.AddScoped<IValidator<EditUserDto>, UserValidator>(); 
    }
}