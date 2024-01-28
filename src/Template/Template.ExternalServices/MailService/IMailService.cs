using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.ExternalServices.MailService
{
    public interface IMailService
    {
        Task<bool> SendEmailAsync<TData>(short templateId, string recipient, TData data);
    }
}
