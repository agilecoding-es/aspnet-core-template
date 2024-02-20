using Template.Infrastructure.EmailService.Smtp.Settings;

namespace Template.Infrastructure.EmailService.AzureCommunicationService.Settings
{
    public class AzureEmailServiceOptions : EmailServiceOptions
    {
        public string AzureCommunicationServiceConnection { get; set; }
    }

}
