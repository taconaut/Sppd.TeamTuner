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

namespace Sppd.TeamTuner.Tests.Integration.DataAccess
{
    /// <summary>
    ///     Sets up and tears down services required for the tests.
    /// </summary>
    internal class TestEnvironmentServiceManager
    {
        /// <summary>
        ///     Gets or sets the service provider.
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        public void Teardown()
        {
            ServiceProvider.GetService<IDatabaseService>().DeleteDatabase();
        }

        /// <summary>
        ///     Sets the configuration by copying Config/appsettings-{provider}.json to Config/appsettings.json
        /// </summary>
        /// <param name="provider">The provider.</param>
        public void SetConfiguration(string provider)
        {
            var configurationSource = $"Config/appsettings-{provider}.json";
            var configurationDest = "Config/appsettings.json";
            File.Copy(configurationSource, configurationDest, true);
        }

        public void Initialize()
        {
            // Instantiate StartupRegistrators registering required services
            var dataAccessStartupRegistrator = new StartupRegistrator();
            var msSqlStartupRegistrator = new Infrastructure.DataAccess.EF.MsSql.StartupRegistrator();
            var sqliteStartupRegistrator = new Infrastructure.DataAccess.EF.Sqlite.StartupRegistrator();
            var inMemoryStartupRegistrator = new Infrastructure.DataAccess.EF.InMemory.StartupRegistrator();
            var infrastructureStartupRegistrator = new Infrastructure.StartupRegistrator();
            var startupRegistrators = new IStartupRegistrator[]
                                      {
                                          infrastructureStartupRegistrator,
                                          msSqlStartupRegistrator,
                                          sqliteStartupRegistrator,
                                          inMemoryStartupRegistrator,
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
            services.AddLogging(logging => { logging.AddLog4Net(Path.Combine(CoreConstants.Config.CONFIG_FOLDER, CoreConstants.Config.LOG4NET_CONFIG_FILE_NAME)); });

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