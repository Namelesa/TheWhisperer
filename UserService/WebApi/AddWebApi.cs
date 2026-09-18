using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserService.Infrastructure.Jwt;
using UserService.WebApi.Auth;
using UserService.WebApi.Auth.Settings;
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

        services.Configure<AuthCookieSettings>(configuration.GetSection("AuthCookie"));
        services.AddScoped<IAuthCookieWriter, AuthCookieWriter>();

        services.AddJwtAuthentication(configuration);

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введите JWT токен"
            });
            c.AddSecurityRequirement(new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    private static void AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.GetSection("Jwt").Bind(jwtSettings);

        var cookieSettings = new AuthCookieSettings();
        configuration.GetSection("AuthCookie").Bind(cookieSettings);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue(
                                cookieSettings.Name, out var token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();
    }
}