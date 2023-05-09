using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;

namespace Template.Configuration
{
    public class AppSettings
    {
        public string ApplicationName { get; set; }
        public SupportedCultures SupportedCultures { get; set; }

        public Mailsettings Mailsettings { get; set; }
        
        public AuthenticationProviders AuthenticationProviders { get; set; }


    }
    public class AuthenticationProviders
    {
        public GoogleOptions GoogleOptions { get; set; }
        public MicrosoftAccountOptions MicrosoftAccountOptions { get; set; }
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

}
