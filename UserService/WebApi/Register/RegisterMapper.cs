using AutoMapper;
using UserService.Application.Register.Dto;
using UserService.WebApi.Register.Contracts;

namespace UserService.WebApi.Register;

public class RegisterMapper : Profile
{
    public RegisterMapper()
    {
        CreateMap<RegisterContract, RegisterDto>();
        CreateMap<RegisterDto, Core.User.User>()
            .ForCtorParam("nickName", opt => opt.MapFrom(src => src.NickName))
            .ForCtorParam("email", opt => opt.MapFrom(src => src.Email))
            .ForCtorParam("image", opt => opt.MapFrom(src => src.Image));
    }
}