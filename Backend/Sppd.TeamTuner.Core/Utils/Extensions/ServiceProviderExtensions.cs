using System;

using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core.Config;

namespace Sppd.TeamTuner.Core.Utils.Extensions
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        ///     Gets the configuration.
        /// </summary>
        /// <typeparam name="TConfig">The type of the configuration.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        public static TConfig GetConfig<TConfig>(this IServiceProvider serviceProvider)
            where TConfig : class, IConfig, new()
        {
            return serviceProvider.GetService<IConfigProvider<TConfig>>().Config;
        }
    }
}