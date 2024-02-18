using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Moq;
using System.Globalization;
using Template.Configuration;
using Template.WebApp.Localization;

namespace Template.WebApp.UnitTests.Localization
{
    public class CultureHelperTests
    {
        private Mock<IOptions<AppSettings>> settings;
        private Mock<IOptions<RequestLocalizationOptions>> localizationOptions;


        public CultureHelperTests()
        {
            settings = new Mock<IOptions<AppSettings>>();
            localizationOptions = new Mock<IOptions<RequestLocalizationOptions>>();
        }

        [Fact]
        public void GetCulture_WhenConfigured_ReturnsCultureInfo()
        {
            settings.Setup(s => s.Value).Returns(new AppSettings());
            localizationOptions.Setup(s => s.Value).Returns(new RequestLocalizationOptions() { SupportedCultures = GetConfiguredCultures() });

            var requestCulture = "es-ES";
            var expectedCulture = new CultureInfo(requestCulture);

            var sut = InitializeCultureHelper();
            var culture = sut.GetCulture(requestCulture);

            Assert.NotNull(culture);
            Assert.Equal(expectedCulture, culture);
        }

        [Fact]
        public void GetCulture_WhenNotConfigured_ReturnsNull()
        {
            settings.Setup(s => s.Value).Returns(new AppSettings());
            localizationOptions.Setup(s => s.Value).Returns(new RequestLocalizationOptions() { SupportedCultures = GetConfiguredCultures() });

            var requestCulture = "es-AR";

            var sut = InitializeCultureHelper();
            var culture = sut.GetCulture(requestCulture);

            Assert.Null(culture);
        }

        private CultureHelper InitializeCultureHelper() => new CultureHelper(settings.Object, localizationOptions.Object);

        private IList<CultureInfo> GetConfiguredCultures() => new[] {
            new CultureInfo("en-US"),
            new CultureInfo("es-ES"),
        };


        //TODO: Completar tests
    }
}
