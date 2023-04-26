using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Configuration
{
    public class AppSettings
    {
        public string ApplicationName { get; set; }

        public Authentication Authentication { get; set; }

        public Mailsettings Mailsettings { get; set; }
    }
    public class Authentication
    {
        public Google Google { get; set; }
        public Microsoft Microsoft { get; set; }
    }

    public class Google
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class Microsoft
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }


    public class Rootobject
    {
        public Mailsettings MailSettings { get; set; }
    }

    public class Mailsettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; }
        public string DisplayName { get; set; }
    }

}
