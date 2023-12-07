using Microsoft.Extensions.Localization;

namespace Template.WebApp.Localization
{
    public class StringLoc<T> : StringLocalizer<T>
    {
        private readonly ILogger _logger;

        public StringLoc(IStringLocalizerFactory stringLocalizerFactory, ILogger<StringLoc<T>> logger) : base(stringLocalizerFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override LocalizedString this[string name]
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

        public override LocalizedString this[string name, params object[] arguments]
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
