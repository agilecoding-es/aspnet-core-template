using Template.Configuration.Setup;

namespace Template.WebApp.Setup
{
    public static class ApplicationInitialize
    {
        public static IAppInitializer CreateAppInitializer(this WebApplication app) => new AppInitializer(app);

        public static async Task<IAppInitializer> DefaultInitialization(this WebApplication app)
        {
            var appInitializer = new AppInitializer(app);

            await Task.WhenAll(new Task[]{
                appInitializer.ApplyMigrations(),
                appInitializer.ConfigureRolesAsync(),
                appInitializer.ConfigureSuperadminAsync()
            });

            return appInitializer;
        }

    }
}
