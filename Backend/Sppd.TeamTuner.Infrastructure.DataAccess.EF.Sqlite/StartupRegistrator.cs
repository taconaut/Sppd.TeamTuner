using System;

using Hangfire;
using Hangfire.SQLite;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Core.Utils.Extensions;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Sqlite
{
    public class StartupRegistrator : IStartupRegistrator
    {
        private const string PROVIDER_NAME = "Sqlite";
        private const string ID_PLACEHOLDER = "{id}";

        public int Priority => 110;

        public void ConfigureServices(IServiceCollection services)
        {
            var databaseConfig = services.BuildServiceProvider().GetConfig<DatabaseConfig>();

            if (PROVIDER_NAME.Equals(databaseConfig.Provider, StringComparison.InvariantCultureIgnoreCase))
            {
                // Add a random string if the {id} specifier is contained in the connection string
                var dbId = Guid.NewGuid().ToString("n").Substring(0, 8);
                databaseConfig.ConnectionString = databaseConfig.ConnectionString.Replace(ID_PLACEHOLDER, dbId);

                services.AddDbContext<TeamTunerContextSqlite>(options => options.UseSqlite(databaseConfig.ConnectionString))
                        .AddScoped<TeamTunerContext, TeamTunerContextSqlite>()
                        .AddScoped<IDatabaseService, TeamTunerContextSqlite>();
            }
        }

        public void Configure(IServiceProvider serviceProvider)
        {
            var generalConfig = serviceProvider.GetConfig<GeneralConfig>();
            var databaseConfig = serviceProvider.GetConfig<DatabaseConfig>();

            if (generalConfig.EnableHangfire && PROVIDER_NAME.Equals(databaseConfig.Provider, StringComparison.InvariantCultureIgnoreCase))
            {
                GlobalConfiguration.Configuration.UseSQLiteStorage(databaseConfig.ConnectionString);
            }
        }
    }
}