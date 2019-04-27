using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;
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
        private const string ID_PLACEHOLDER = "{id}";

        public int Priority => 100;

        public void Register(IServiceCollection services)
        {
            var databaseConfig = services.BuildServiceProvider().GetConfig<DatabaseConfig>();

            // DB context
            var dbId = Guid.NewGuid().ToString("n").Substring(0, 8);
            databaseConfig.ConnectionString = databaseConfig.ConnectionString.Replace(ID_PLACEHOLDER, dbId);
            services.AddDbContext<TeamTunerContext>(options => options.UseSqlServer(databaseConfig.ConnectionString))
                    .AddScoped<IDatabaseService, TeamTunerContext>();

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>))
                    .AddScoped<ITeamTunerUserRepository, TeamTunerUserRepository>()
                    .AddScoped<ITeamRepository, TeamRepository>()
                    .AddScoped<ITeamMembershipRequestRepository, TeamMembershipRequestRepository>()
                    .AddScoped<ICardLevelRepository, CardLevelRepository>();

            // Meta data providers
            services.AddScoped<IEntityMetadataProvider, BaseEntityMetadataProvider>();

            // Validation
            services.AddScoped<IValidationService, ValidationService>()
                    .AddScoped<IValidationContext, ValidationContext>();

            // Unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Only register seeders if they are going to be used
            if (databaseConfig.AutoMigrate && databaseConfig.SeedMode.MustSeedRequired())
            {
                services.AddScoped<IDbSeeder, RarityDbSeeder>()
                        .AddScoped<IDbSeeder, CardThemeDbSeeder>()
                        .AddScoped<IDbSeeder, CardDbSeeder>()
                        .AddScoped<IDbSeeder, CardTypeDbSeeder>();

                if (databaseConfig.SeedMode == SeedMode.Test)
                {
                    services.AddScoped<IDbSeeder, TeamSeeder>()
                            .AddScoped<IDbSeeder, FederationSeeder>()
                            .AddScoped<IDbSeeder, TeamTunerUserSeeder>();
                }
            }
        }

        public void Configure(IServiceProvider serviceProvider)
        {
            var databaseConfig = serviceProvider.GetConfig<DatabaseConfig>();
            if (databaseConfig.AutoMigrate)
            {
                serviceProvider.InitializeDatabase();
            }
        }
    }
}