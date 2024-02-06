using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using System.Text;
using System.Text.RegularExpressions;

namespace Template.Configuration
{

    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }

        public DbOptions Database { get; set; }

        public AuthenticationProvidersOptions AuthenticationProviders { get; set; }

        public HealthChecksOptions HealthChecks { get; set; }

        public LoggingExceptionsOptions LoggingExceptions { get; set; }

        public ErrorsOptions Errors { get; set; }

        public MessagesOptions Messages { get; set; }

        public ExternalEmailServiceOptions ExternalEmailService { get; set; }

        public SupportedCulturesOptions SupportedCultures { get; set; }

        public SamplesOptions Samples { get; set; }

        public string ApplicationName { get; set; }

        public string SuperadminPass { get; set; }

        public bool LogHttpEnabled { get; set; } = false;
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }

    }


    public class AuthenticationProvidersOptions
    {
        public const string Key = "AuthenticationProviders";

        public GoogleOptions Google { get; set; }
        public MicrosoftAccountOptions Microsoft { get; set; }
    }

    public class DbOptions
    {
        public const string Key = "Db";

        public string Provider { get; set; }
    }

    public class HealthChecksOptions
    {
        public const string Key = "HealthChecks";

        public bool Enabled { get; set; }
        public LatencyHealthCheck LatencyHealthCheck { get; set; }
    }

    public class LatencyHealthCheck
    {
        public int OkLatency { get; set; }
        public int DegradedLatency { get; set; }
    }

    public class LoggingExceptionsOptions
    {
        public const string Key = "LoggingExceptions";

        public bool Enabled { get; set; }
        public bool Rethrow { get; set; }
    }

    public class ErrorsOptions
    {
        public const string Key = "Errors";

        public bool ShowRequestId { get; set; }
    }

    public class MessagesOptions
    {
        public const string Key = "Messages";

        public bool TreatValidationsAsWarnings { get; set; }
    }

    public class ExternalEmailServiceOptions
    {
        public const string Key = "ExternalEmailService";

        public bool Enabled { get; set; }
        public string ServiceName { get; set; }
        public string Url { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }

    public class SupportedCulturesOptions
    {
        public const string Key = "SupportedCultures";

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

    public class SamplesOptions
    {
        public const string Key = "Samples";

        public bool ShowSampleMVC { get; set; }
        public bool ShowSampleAjax { get; set; }
    }
}
