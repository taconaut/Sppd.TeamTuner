using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Core.Utils.Helpers;
using Sppd.TeamTuner.Infrastructure.Config;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF;

namespace Sppd.TeamTuner.Tests.Integration
{
    /// <summary>
    ///     Base class for repository tests which configures the services required for the tests.
    /// </summary>
    public abstract class RepositoryTestsBase : IDisposable
    {
        /// <summary>
        ///     Gets or sets the service provider.
        /// </summary>
        protected IServiceProvider ServiceProvider { get; set; }

        protected RepositoryTestsBase()
        {
            // Instantiate StartupRegistrators registering required services
            var dataAccessStartupRegistrator = new StartupRegistrator();
            var msSqlStartupRegistrator = new Infrastructure.DataAccess.EF.MsSql.StartupShutdownRegistrator();
            var sqliteStartupRegistrator = new Infrastructure.DataAccess.EF.Sqlite.StartupShutdownRegistrator();
            var infrastructureStartupRegistrator = new Infrastructure.StartupRegistrator();
            var startupRegistrators = new IStartupRegistrator[]
                                      {
                                          infrastructureStartupRegistrator,
                                          msSqlStartupRegistrator,
                                          sqliteStartupRegistrator,
                                          dataAccessStartupRegistrator
                                      };

            // Register services
            var services = new ServiceCollection();
            RegisterServices(services, startupRegistrators);

            // Build the service provider
            ServiceProvider = services.BuildServiceProvider();

            // Configure
            ConfigureServices(startupRegistrators);
        }

        public void Dispose()
        {
            //ServiceProvider.GetService<IDatabaseService>().DeleteDatabase();
        }

        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                   .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), CoreConstants.Config.CONFIG_FOLDER))
                   .AddJsonFile(CoreConstants.Config.APP_CONFIG_FILE_NAME, false, false)
                   .Build();
        }

        private static void RegisterServices(IServiceCollection services, IEnumerable<IStartupRegistrator> startupRegistrators)
        {
            // Logging
            services.AddLogging(logging =>
            {
                logging.AddLog4Net(Path.Combine(CoreConstants.Config.CONFIG_FOLDER, CoreConstants.Config.LOG4NET_CONFIG_FILE_NAME));
                logging.SetMinimumLevel(LogLevel.Trace);
            });

            // Configuration
            services.AddSingleton(typeof(IConfigProvider<>), typeof(ConfigProvider<>));
            services.AddSingleton(BuildConfiguration());

            // Lazy
            services.AddScoped(typeof(Lazy<>));

            foreach (var startupRegistrator in startupRegistrators)
            {
                startupRegistrator.Register(services);
            }
        }

        private void ConfigureServices(IEnumerable<IStartupRegistrator> startupRegistrators)
        {
            var loggerFactory = ServiceProvider.GetService<ILoggerFactory>();
            LoggerHelper.LoggerFactory = loggerFactory;

            foreach (var startupRegistrator in startupRegistrators)
            {
                startupRegistrator.Configure(ServiceProvider);
            }
        }
    }
}