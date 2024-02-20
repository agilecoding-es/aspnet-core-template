using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.EmailService.Smtp.Settings
{
    public class EmailServiceOptions
    {
        public const string Key = "EmailService";

        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; }
        public string DisplayName { get; set; }
    }

}
