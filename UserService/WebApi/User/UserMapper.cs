using AutoMapper;
using UserService.Application.User.Dto;

namespace UserService.WebApi.User;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<Contracts.EditUserContract, EditUserDto>()
            .ForMember(dest => dest.NewNickName, opt => opt.MapFrom(src => src.NewNickName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));
        CreateMap<Contracts.EditUserPasswordContract, EditUserPasswordDto>();
        CreateMap<Core.User.User, UserDto>()
            .ForMember(dest => dest.NickName, opt => opt.MapFrom(src => src.NickName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));
    }
}