using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Common;
using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Core.Utils.Extensions;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.InMemory
{
    public class StartupRegistrator : IStartupRegistrator
    {
        private const string PROVIDER_NAME = "InMemory";

        public int Priority => 90;

        public void Register(IServiceCollection services)
        {
            var databaseConfig = services.BuildServiceProvider().GetConfig<DatabaseConfig>();

            if (PROVIDER_NAME.Equals(databaseConfig.Provider, StringComparison.InvariantCultureIgnoreCase))
            {
                // Add a random string to have a unique database name
                var dbId = Guid.NewGuid().ToString("n").Substring(0, 8);
                services.AddDbContext<TeamTunerContextInMemory>(options => options.UseInMemoryDatabase($"Sppd.TeamTuner.InMemory-{dbId}"))
                        .AddScoped<TeamTunerContext, TeamTunerContextInMemory>()
                        .AddScoped<IDatabaseService, TeamTunerContextInMemory>();
            }
        }

        public void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}