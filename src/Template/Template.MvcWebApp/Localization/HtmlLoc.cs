using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace Template.MvcWebApp.Localization
{
    public class HtmlLoc : HtmlLocalizer
    {
        private readonly ILogger<HtmlLoc> _logger;

        public HtmlLoc(IStringLocalizer localizer, ILogger<HtmlLoc> logger) : base(localizer)
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

    public class HtmlLoc<T> : HtmlLocalizer<T>
    {
        private readonly ILogger _logger;

        public HtmlLoc(IHtmlLocalizerFactory htmlLocalizerFactory, ILogger<HtmlLoc<T>> logger) : base(htmlLocalizerFactory)
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
