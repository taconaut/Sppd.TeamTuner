using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Domain;
using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Providers;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Core.Utils.Helpers;
using Sppd.TeamTuner.Core.Validation;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Extensions;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Providers;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Repositories;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Seeders;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Validation;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF
{
    public class StartupRegistrator : IStartupRegistrator
    {
        private static readonly ILogger<StartupRegistrator> s_logger = LoggerHelper.CreateLogger<StartupRegistrator>();

        public int Priority => 100;

        public void Register(IServiceCollection services)
        {
            var databaseConfig = GetDatabaseConfig(services.BuildServiceProvider());

            // DB context
            services.AddDbContext<TeamTunerContext>(options => options.UseSqlServer(databaseConfig.ConnectionString));

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITeamTunerUserRepository, TeamTunerUserRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();

            // Meta data providers
            services.AddScoped<IEntityMetadataProvider, BaseEntityMetadataProvider>();

            // Validation
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<IValidationContext, ValidationContext>();

            // Unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Only register seeders if they are going to be used
            if (databaseConfig.SeedMode.MustSeedRequired())
            {
                services.AddScoped<IDbSeeder, RarityDbSeeder>();
                services.AddScoped<IDbSeeder, CardThemeDbSeeder>();
                services.AddScoped<IDbSeeder, CardDbSeeder>();
                services.AddScoped<IDbSeeder, CardTypeDbSeeder>();

                if (databaseConfig.SeedMode == SeedMode.Test)
                {
                    services.AddScoped<IDbSeeder, TeamSeeder>();
                    services.AddScoped<IDbSeeder, FederationSeeder>();
                    services.AddScoped<IDbSeeder, TeamTunerUserSeeder>();
                }
            }
        }

        public void Configure(IServiceProvider serviceProvider)
        {
            var databaseConfig = GetDatabaseConfig(serviceProvider);
            if (databaseConfig.AutoMigrate)
            {
                InitializeDatabase(serviceProvider);
            }
        }

        private static void InitializeDatabase(IServiceProvider serviceProvider)
        {
            var databaseConfig = GetDatabaseConfig(serviceProvider);

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
                    s_logger.LogInformation($"Pending migrations have been applied: {string.Join(", ", pendingMigration)}");
                }
                else
                {
                    s_logger.LogDebug("Database is already up to date.");
                }

                if (isNewDatabase)
                {
                    s_logger.LogDebug($"New database created. Seed data for SeedMode={databaseConfig.SeedMode}");

                    var seedTasks = new List<Task>();
                    foreach (var seeder in scope.ServiceProvider.GetServices<IDbSeeder>().OrderBy(seeder => seeder.Priority))
                    {
                        seedTasks.Add(seeder.SeedAsync());
                        s_logger.LogDebug($"Seeded {seeder.GetType().Name}");
                    }

                    Task.WaitAll(seedTasks.ToArray());

                    // The changes are usually being saved by a unit of work. Here, while starting the application, we will do it on the context itself.
                    context.SaveChanges();
                }
            }

            s_logger.LogInformation("Database has been initialized");
        }

        private static DatabaseConfig GetDatabaseConfig(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<IConfigProvider<DatabaseConfig>>().Config;
        }
    }
}