﻿using System;

using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Core.Utils.Extensions;
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

        public void ConfigureServices(IServiceCollection services)
        {
            var databaseConfig = services.BuildServiceProvider().GetConfig<DatabaseConfig>();

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>))
                    .AddScoped<ITeamTunerUserRepository, TeamTunerUserRepository>()
                    .AddScoped<ITeamRepository, TeamRepository>()
                    .AddScoped<ITeamMembershipRequestRepository, TeamMembershipRequestRepository>()
                    .AddScoped<ICardLevelRepository, CardLevelRepository>()
                    .AddScoped<ICardRepository, CardRepository>()
                    .AddScoped<IRegistrationRequestRepository, RegistrationRequestRepository>();

            // Meta data providers
            services.AddScoped<IEntityMetadataProvider, BaseEntityMetadataProvider>();

            // Validation
            services.AddScoped<IValidationService, ValidationService>()
                    .AddScoped<IValidationContext, ValidationContext>();

            // Unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Only register seeders if they are going to be used
            if (databaseConfig.ManageDatabaseSchema && databaseConfig.SeedMode.MustSeedRequired())
            {
                services.AddScoped<IDbSeeder, RarityDbSeeder>()
                        .AddScoped<IDbSeeder, CardThemeDbSeeder>()
                        .AddScoped<IDbSeeder, CardDbSeeder>()
                        .AddScoped<IDbSeeder, CardTypeDbSeeder>()
                        .AddScoped<IDbSeeder, CharacterTypeDbSeeder>()
                        .AddScoped<IDbSeeder, TeamTunerUserSeeder>();

                if (databaseConfig.SeedMode == SeedMode.Test)
                {
                    services.AddScoped<IDbSeeder, TeamSeeder>()
                            .AddScoped<IDbSeeder, FederationSeeder>();
                }
            }
        }

        public void Configure(IServiceProvider serviceProvider)
        {
            var databaseConfig = serviceProvider.GetConfig<DatabaseConfig>();
            if (databaseConfig.ManageDatabaseSchema)
            {
                serviceProvider.InitializeDatabase();
            }
        }
    }
}