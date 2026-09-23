using AutoMapper;
using NotificationService.Core.UserDelete;
using NotificationService.WebApi.UserDelete.Contracts;
using SharedModels.User.UserDelete;

namespace NotificationService.WebApi.UserDelete;

public class UserDeleteMapper : Profile
{
    public UserDeleteMapper()
    {
        CreateMap<UserDeleteContract, UserDeleteModel>()
            .ForMember(dest => dest.NickName, opt => opt.MapFrom(src => src.NickName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        CreateMap<UserDeleteByEmailModel, UserDeleteModel>()
            .ForMember(dest => dest.NickName, opt => opt.MapFrom(src => src.NickName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
    }
}