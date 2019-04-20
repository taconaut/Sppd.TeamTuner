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
        public int Priority => 100;

        public void Register(IServiceCollection services)
        {
            var databaseConfig = services.BuildServiceProvider().GetConfig<DatabaseConfig>();

            // DB context
            services.AddDbContext<TeamTunerContext>(options => options.UseSqlServer(databaseConfig.ConnectionString));

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITeamTunerUserRepository, TeamTunerUserRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ITeamMembershipRequestRepository, TeamMembershipRequestRepository>();

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
            var databaseConfig = serviceProvider.GetConfig<DatabaseConfig>();
            if (databaseConfig.AutoMigrate)
            {
                serviceProvider.InitializeDatabase();
            }
        }
    }
}