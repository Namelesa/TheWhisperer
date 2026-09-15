using AutoMapper;
using UserService.Application.PasswordRecovery.Dto;
using UserService.Core.ForgotPassword;
using UserService.WebApi.ForgotPassword.Contracts;

namespace UserService.WebApi.ForgotPassword;

public class ForgotPasswordMapper : Profile
{
    public ForgotPasswordMapper()
    {
        CreateMap<ForgotPasswordContract, ForgotPasswordDto>();
        CreateMap<ForgotPasswordDto, UserForgotPassword >();
        CreateMap<ResetPasswordContract, ResetPasswordDto>();
    }
}