using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using System.Text;
using System.Text.RegularExpressions;

namespace Template.Configuration
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }

        public Db Database { get; set; }

        public AuthenticationProviders AuthenticationProviders { get; set; }

        public Mailsettings Mailsettings { get; set; }

        public HealthChecks HealthChecks { get; set; }

        public LogMiddleware LogMiddleware { get; set; }

        public SupportedCultures SupportedCultures { get; set; }

        public Errors Errors { get; set; }

        public Messages Messages { get; set; }

        public Samples Samples { get; set; }

        public string ApplicationName { get; set; }

        public string SuperadminPass { get; set; }

        public bool LogHttpEnabled { get; set; } = false;
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
        public string HealthChecksConnection { get; set; }
        public string AzureCommunicationServiceConnection { get; set; }

    }

    public class Db
    {
        public string Provider { get; set; }
    }

    public class AuthenticationProviders
    {
        public GoogleOptions Google { get; set; }
        public MicrosoftAccountOptions Microsoft { get; set; }
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

    public class HealthChecks
    {
        public bool Enabled { get; set; }
        public LatencyHealthCheck LatencyHealthCheck { get; set; }
    }

    public class LatencyHealthCheck
    {
        public int OkLatency { get; set; }
        public int DegradedLatency { get; set; }
    }

    public class LogMiddleware
    {
        public bool Enabled { get; set; }
        public bool Rethrow { get; set; }
    }

    public class SupportedCultures
    {
        public List<Culture> Cultures { get; set; }

        public string DefaultCulture { get; set; }

        public int CookieLifeTimeDays { get; set; }

        public List<string> GetSupportedCultures()
        {
            var cultures = new List<string>();

            foreach (var culture in Cultures)
                cultures.AddRange(culture.GetCultures());

            return cultures;
        }

        public string GetValidCulture(string culture)
        {
            var pattern = GetRegularExpression();
            var regex = new Regex(pattern);

            var result = regex.Match(culture ?? string.Empty);

            string language = null, country = null;
            if (result.Success)
            {
                language = result.Groups.GetValueOrDefault("language")?.Value;
                country = result.Groups.GetValueOrDefault("country")?.Value;

                if (string.IsNullOrEmpty(country))
                    culture = $"{language}-{Cultures.First(c => c.Language == language).DefaultCountry}";
            }
            else
            {
                culture = DefaultCulture;
            }

            return culture;
        }

        public string GetRegularExpression()
        {
            var languageExpression = new StringBuilder();

            foreach (var language in Cultures)
                languageExpression.Append($"{language.GetRegularExpression()}|");

            languageExpression.Remove(languageExpression.Length - 1, 1);

            return $"^(?<culture>{languageExpression.ToString()})$";
        }
    }

    public class Culture
    {
        string _defaultCountry;

        public string Language { get; set; }
        public string[] Countries { get; set; }
        public string DefaultCountry
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_defaultCountry))
                    return _defaultCountry;
                else
                    return Countries.FirstOrDefault();
            }
            set => _defaultCountry = value;
        }

        public List<string> GetCultures()
        {
            var languages = new List<string>() {
                Language
            };
            if (Countries.Length > 0)
            {
                foreach (var country in Countries)
                    languages.Add($"{Language}-{country}");
            }
            return languages;
        }
        public string GetDefaultCulture() => !string.IsNullOrWhiteSpace(DefaultCountry) ? $"{Language}-{DefaultCountry}" : Language;
        public string GetRegularExpression()
        {
            var countriesExpression = new StringBuilder();

            foreach (var country in Countries)
                countriesExpression.Append($"{country}|");
            countriesExpression.Remove(countriesExpression.Length - 1, 1);

            return $"(?<language>{Language})(?:-(?<country>{countriesExpression.ToString()}))?|(?<language>{Language})(?:-\\w+)?";
        }
    }

    public class Errors
    {
        public bool ShowRequestId { get; set; }
    }

    public class Messages
    {
        public bool TreatValidationsAsWarnings { get; set; }
    }

    public class Samples
    {
        public bool ShowSampleMVC { get; set; }
        public bool ShowSampleAjax { get; set; }
    }
}
