using Microsoft.AspNetCore.Mvc.Localization;

namespace Template.MvcWebApp.Localization
{
    public class Localizer<T> : HtmlLocalizer<T>
    {
        private readonly ILogger<T> _logger;

        public Localizer(IHtmlLocalizerFactory htmlLocalizerFactory, ILogger<T> logger) : base(htmlLocalizerFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override LocalizedHtmlString this[string name]
        {
            get
            {
                try
                {
                    return base[name];
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Resource: {name}");
                    return default;
                }
            }
        }

        public override LocalizedHtmlString this[string name, params object[] arguments]
        {
            get
            {
                try
                {
                    return base[name, arguments];
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Resource: {name}", arguments);
                    return default;
                }
            }
        }
    }
}
