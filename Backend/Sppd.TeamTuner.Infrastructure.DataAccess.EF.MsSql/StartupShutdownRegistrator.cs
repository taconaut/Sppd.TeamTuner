using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Domain.Enumerations;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Core.Utils.Extensions;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.MsSql
{
    public class StartupShutdownRegistrator : IStartupRegistrator, IShutdownRegistrator
    {
        private const string PROVIDER_NAME = "MsSql";
        private const string ID_PLACEHOLDER = "{id}";

        private IServiceProvider _serviceProvider;

        public void OnBeforeShutdown()
        {
            var databaseConfig = _serviceProvider.GetConfig<DatabaseConfig>();
            if (!PROVIDER_NAME.Equals(databaseConfig.Provider, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var generalConfig = _serviceProvider.GetConfig<GeneralConfig>();
            if (generalConfig.ExecutionMode == ExecutionMode.Test)
            {
                _serviceProvider.GetService<IDatabaseService>().DeleteDatabase();
            }
        }

        public int Priority => 90;

        public void Register(IServiceCollection services)
        {
            var databaseConfig = services.BuildServiceProvider().GetConfig<DatabaseConfig>();

            if (PROVIDER_NAME.Equals(databaseConfig.Provider, StringComparison.InvariantCultureIgnoreCase))
            {
                // Add a random string if the {id} specifier is contained in the connection string
                var dbId = Guid.NewGuid().ToString("n").Substring(0, 8);
                databaseConfig.ConnectionString = databaseConfig.ConnectionString.Replace(ID_PLACEHOLDER, dbId);

                services.AddDbContext<TeamTunerContextMsSql>(options => options.UseSqlServer(databaseConfig.ConnectionString))
                        .AddScoped<TeamTunerContext, TeamTunerContextMsSql>()
                        .AddScoped<IDatabaseService, TeamTunerContextMsSql>();
            }
        }

        public void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}