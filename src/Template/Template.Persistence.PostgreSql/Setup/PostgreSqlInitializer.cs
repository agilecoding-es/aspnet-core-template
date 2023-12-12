using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Template.Persistence.PosgreSql.Database;

namespace Template.Configuration.Setup
{
    public static class PostgreSqlInitializer
    {
        public static async Task<IAppInitializer> ApplyMigrations(this IAppInitializer appInitializer) {

            using (var scope = appInitializer.App.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<Context>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Context>>();

                try
                {
                    logger.LogInformation($"[Database] Applying migrations for context: {typeof(Context).Name}");

                    await db.Database.MigrateAsync();

                    logger.LogInformation($"[Database] Migrations applied for context: {typeof(Context).Name}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while applying migrations for context: {typeof(Context).Name}.");
                }
            }

            return appInitializer;
        }
    }
}
