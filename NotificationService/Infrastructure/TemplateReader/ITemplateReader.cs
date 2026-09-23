namespace NotificationService.Infrastructure.TemplateReader;

public interface ITemplateReader
{
    Task<string?> ReadTemplateAsync(string templatePath);
}