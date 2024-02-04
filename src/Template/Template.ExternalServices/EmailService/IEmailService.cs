using Template.ExternalServices.EmailService.DTOs;

namespace Template.ExternalServices.EmailService
{
    public interface IEmailService
    {
        Task<List<EmailTemplateDto>> GetTemplatesAsync();
        Task<EmailTemplateDto> GetTemplateAsync(int templateId);
        Task<string> PreviewemplateAsync(int templateId);
        Task<bool> SendEmailAsync<TData>(int templateId, string recipient, TData data);
    }
}
