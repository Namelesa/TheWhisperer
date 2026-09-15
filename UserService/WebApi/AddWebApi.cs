using UserService.WebApi.ForgotPassword;
using UserService.WebApi.Login;
using UserService.WebApi.Register;
using UserService.WebApi.User;

namespace UserService.WebApi;

public static class AddWebApi
{
    public static void AddWebApiLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(config => config.AddProfile(new RegisterMapper()));
        services.AddAutoMapper(config => config.AddProfile(new LoginMapper()));
        services.AddAutoMapper(config => config.AddProfile(new UserMapper()));
        services.AddAutoMapper(config => config.AddProfile(new ForgotPasswordMapper()));
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}