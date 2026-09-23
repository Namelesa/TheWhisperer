using NotificationService.WebApi.UserDelete;
using NotificationService.WebApi.UserInfo;

namespace NotificationService.WebApi;

public static class AddWebApi
{
    public static void AddWebApiLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    { 
        services.AddAutoMapper(config => config.AddProfile(new UserInfoMapper()));
        services.AddAutoMapper(config => config.AddProfile(new UserDeleteMapper()));
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}