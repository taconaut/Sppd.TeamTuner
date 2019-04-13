using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Domain;
using Sppd.TeamTuner.Core.Providers;
using Sppd.TeamTuner.Core.Utils.Helpers;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Seeders;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Extensions
{
    internal static class ServiceProviderExtensions
    {
        public static void InitializeDatabase(this IServiceProvider serviceProvider)
        {
            var logger = LoggerHelper.LoggerFactory.CreateLogger("DatabaseInitializer");

            var databaseConfig = serviceProvider.GetConfig<DatabaseConfig>();

            using (var scope = serviceProvider.CreateScope())
            {
                // Set SystemUser as the current user to properly set createdBy and updatedBy properties when persisting seeded data to DB
                var userProvider = scope.ServiceProvider.GetService<ITeamTunerUserProvider>();
                userProvider.CurrentUser = new SystemUser();

                var context = scope.ServiceProvider.GetRequiredService<TeamTunerContext>();

                if (databaseConfig.DeleteOnStartup)
                {
                    context.Database.EnsureDeleted();
                }

                var isNewDatabase = !context.Database.GetService<IRelationalDatabaseCreator>().Exists();

                var pendingMigration = context.Database.GetPendingMigrations().ToList();
                if (pendingMigration.Any())
                {
                    context.Database.Migrate();
                    logger.LogInformation($"Pending migrations have been applied: {string.Join(", ", pendingMigration)}");
                }
                else
                {
                    logger.LogDebug("Database is already up to date.");
                }

                if (isNewDatabase)
                {
                    logger.LogDebug($"New database created. Seed data for SeedMode={databaseConfig.SeedMode}");

                    var seedTasks = new List<Task>();
                    foreach (var seeder in scope.ServiceProvider.GetServices<IDbSeeder>().OrderBy(seeder => seeder.Priority))
                    {
                        seedTasks.Add(seeder.SeedAsync());
                        logger.LogDebug($"Seeded {seeder.GetType().Name}");
                    }

                    Task.WaitAll(seedTasks.ToArray());

                    // The changes are usually being saved by a unit of work. Here, while starting the application, we will do it on the context itself.
                    context.SaveChangesAsync().Wait();
                }
            }

            logger.LogInformation("Database has been initialized");
        }

        public static TConfig GetConfig<TConfig>(this IServiceProvider serviceProvider)
            where TConfig : class, IConfig, new()
        {
            return serviceProvider.GetService<IConfigProvider<TConfig>>().Config;
        }
    }
}