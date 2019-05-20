using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Sppd.TeamTuner.Core.Domain;
using Sppd.TeamTuner.Core.Providers;
using Sppd.TeamTuner.Core.Utils.Extensions;
using Sppd.TeamTuner.Core.Utils.Helpers;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Seeders;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Extensions
{
    internal static class ServiceProviderExtensions
    {
        public static void InitializeDatabase(this IServiceProvider serviceProvider)
        {
            var logger = LoggerHelper.LoggerFactory.CreateLogger("Sppd.TeamTuner.Infrastructure.DataAccess.EF.DatabaseInitializer");

            var databaseConfig = serviceProvider.GetConfig<DatabaseConfig>();

            using (var scope = serviceProvider.CreateScope())
            {
                // Set SystemUser as the current user to properly set createdBy and updatedBy properties when persisting seeded data to DB
                var userProvider = scope.ServiceProvider.GetService<ITeamTunerUserProvider>();
                userProvider.CurrentUser = new SystemUser();

                var context = scope.ServiceProvider.GetService<TeamTunerContext>();

                if (databaseConfig.DeleteOnStartup)
                {
                    context.Database.EnsureDeleted();
                }

                bool seedData;
                if (databaseConfig.Initialize)
                {
                    seedData = !context.Database.GetService<IRelationalDatabaseCreator>().Exists();

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
                }
                else
                {
                    // Assume true (only known case is InMemory database provider)
                    seedData = true;
                }

                if (seedData)
                {
                    logger.LogDebug($"Seed data for SeedMode={databaseConfig.SeedMode}");

                    var seedTasks = new List<Task>();
                    var seeders = scope.ServiceProvider.GetServices<IDbSeeder>().OrderBy(seeder => seeder.Priority).ToList();
                    foreach (var seeder in seeders)
                    {
                        seedTasks.Add(seeder.SeedAsync());
                    }

                    Task.WaitAll(seedTasks.ToArray());
                    logger.LogInformation($"Finished seeding {string.Join(", ", seeders.Select(s => s.GetType().Name))}");

                    // The changes are usually being saved by a unit of work. Here, while starting the application, we will do it on the context itself.
                    context.SaveChangesAsync().Wait();
                }
            }

            logger.LogInformation("Database has been initialized");
        }
    }
}