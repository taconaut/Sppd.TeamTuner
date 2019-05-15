using System;

using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Providers;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Infrastructure.Config;
using Sppd.TeamTuner.Infrastructure.Providers;
using Sppd.TeamTuner.Infrastructure.Resolvers;
using Sppd.TeamTuner.Infrastructure.Services;

namespace Sppd.TeamTuner.Infrastructure
{
    public class StartupRegistrator : IStartupRegistrator
    {
        public int Priority => int.MinValue;

        public void Register(IServiceCollection services)
        {
            services.AddSingleton(typeof(IConfigProvider<>), typeof(ConfigProvider<>));

            // Services
            services.AddScoped<ITeamTunerUserService, TeamTunerUserService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<ICoreDataService, CoreDataService>();

            // Providers
            services.AddScoped<ITeamTunerUserProvider, TeamTunerUserProvider>();

            // Resolvers
            services.AddTransient<IRepositoryResolver, RepositoryResolver>();
        }

        public void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}