using AutoMapper;
using UserService.WebApi.Login.Contracts;

namespace UserService.WebApi.Login;

public class LoginMapper : Profile
{
    public LoginMapper()
    {
        CreateMap<LoginContract, Application.Login.Dto.LoginDto>();
    }
}