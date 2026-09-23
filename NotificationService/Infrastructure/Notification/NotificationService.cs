using Microsoft.AspNetCore.Identity.UI.Services;
using NotificationService.Core.UserDelete;
using NotificationService.Core.UserInfo;
using NotificationService.Infrastructure.MailJet;
using NotificationService.Infrastructure.TemplateReader;

namespace NotificationService.Infrastructure.Notification
{
    public class NotificationService(IEmailSender emailSender, ITemplateReader templateReader) : INotificationService
    {
        public async Task<bool> SendConfirmEmailAsync(UserInfoModel userDto, string link)
        {
            var htmlBody = await templateReader.ReadTemplateAsync(Wc.ConfirmEmailTemplate);
            if (htmlBody == null) return false;

            htmlBody = htmlBody.Replace("{UserName}", userDto.NickName)
                .Replace("{link}", link);

            await emailSender.SendEmailAsync(userDto.Email, Wc.ConfirmEmail, htmlBody);
            return true;
        }

        public async Task<bool> SendEditUserInfoEmailAsync(UserInfoModel userDto)
        {
            var htmlBody = await templateReader.ReadTemplateAsync(Wc.EditUserTemplate);
            if (htmlBody == null) return false;

            htmlBody = htmlBody.Replace("{UserName}", userDto.NickName);

            await emailSender.SendEmailAsync(userDto.Email, Wc.EditUser, htmlBody);
            return true;
        }

        public async Task<bool> SendDeleteUserEmailAsync(UserDeleteModel userDto)
        {
            var htmlBody = await templateReader.ReadTemplateAsync(Wc.DeleteUserTemplate);
            if (htmlBody == null) return false;

            htmlBody = htmlBody.Replace("{UserName}", userDto.NickName);

            await emailSender.SendEmailAsync(userDto.Email, Wc.DeleteUser, htmlBody);
            return true;
        }
    }
}