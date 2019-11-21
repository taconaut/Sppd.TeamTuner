using System;

using Hangfire;

using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Utils.Extensions;

namespace Sppd.TeamTuner.Infrastructure.Jobs
{
    public class StartupRegistrator : IStartupRegistrator
    {
        public int Priority => 150;

        public void ConfigureServices(IServiceCollection services)
        {
            var generalConfig = services.BuildServiceProvider().GetConfig<GeneralConfig>();
            if (!generalConfig.EnableHangfire)
            {
                return;
            }

            // Hangfire database connection is being registered in the provider project. Pass it as parameter here.
            // ReSharper disable once RedundantAssignment
            services.AddHangfire(config => config = GlobalConfiguration.Configuration);
            services.AddHangfireServer();

            // Jobs
            services.AddTransient<IJobManager, JobManager>();
        }

        public void Configure(IServiceProvider serviceProvider)
        {
            var generalConfig = serviceProvider.GetConfig<GeneralConfig>();
            if (!generalConfig.EnableHangfire)
            {
                return;
            }

            serviceProvider.GetService<IJobManager>()
                           .RegisterCardImportJob();
        }
    }
}