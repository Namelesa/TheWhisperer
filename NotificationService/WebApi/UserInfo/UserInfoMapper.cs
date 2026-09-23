using AutoMapper;
using NotificationService.Core.UserInfo;
using NotificationService.WebApi.UserInfo.Contracts;
using SharedModels.User.UserConfirmEmail;

namespace NotificationService.WebApi.UserInfo;

public class UserInfoMapper : Profile
{
    public UserInfoMapper()
    {
        CreateMap<UserInfoContract, UserInfoModel>()
            .ForMember(dest => dest.NickName, opt => opt.MapFrom(src => src.NickName))
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        CreateMap<UserEmailConfirmModel, UserInfoModel>()
            .ForMember(dest => dest.NickName, opt => opt.MapFrom(src => src.NickName))
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
    }
}