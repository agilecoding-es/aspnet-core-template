using Template.Infrastructure.EmailService.Smtp.Settings;

namespace Template.Infrastructure.EmailService.AzureCommunicationService.Settings
{
    public class AzureMailSettingOptions : MailSettingOptions
    {
        public string AzureCommunicationServiceConnection { get; set; }
    }

}
