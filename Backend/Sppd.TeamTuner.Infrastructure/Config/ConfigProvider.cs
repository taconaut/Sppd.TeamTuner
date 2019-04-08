using System;
using System.Threading;

using Microsoft.Extensions.Configuration;

using Sppd.TeamTuner.Core.Config;

namespace Sppd.TeamTuner.Infrastructure.Config
{
    /// <summary>
    ///     Provider that delivers populated <see cref="IConfig" /> objects.
    /// </summary>
    /// <typeparam name="TConfig">Type of the configuration</typeparam>
    /// <seealso cref="IConfigProvider{TConfig}" />
    public class ConfigProvider<TConfig> : IConfigProvider<TConfig>
        where TConfig : class, IConfig, new()
    {
        private readonly Lazy<TConfig> _config;
        private readonly IConfiguration _configuration;

        public ConfigProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            _config = new Lazy<TConfig>(CreateBoundConfig, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        ///     Gets the configuration
        /// </summary>
        public TConfig Config => _config.Value;

        /// <summary>
        ///     Binds the configuration to the configuration file.
        /// </summary>
        private TConfig CreateBoundConfig()
        {
            var configValues = new TConfig();

            if (string.IsNullOrWhiteSpace(configValues.SectionKey))
            {
                _configuration.Bind(configValues);
            }
            else
            {
                _configuration.Bind(configValues.SectionKey, configValues);
            }

            return configValues;
        }
    }
}