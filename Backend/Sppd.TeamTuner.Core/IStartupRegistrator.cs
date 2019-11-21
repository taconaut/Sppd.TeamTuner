using System;

using Microsoft.Extensions.DependencyInjection;

namespace Sppd.TeamTuner.Core
{
    /// <summary>
    ///     Implement this interface to register dependencies and/or serviceCollection during application startup. Note that
    ///     classes implementing this interface will be called automatically.
    /// </summary>
    public interface IStartupRegistrator
    {
        /// <summary>
        ///     Gets the priority. <see cref="IStartupRegistrator" /> with a lower <see cref="Priority" /> will be executed first.
        /// </summary>
        int Priority { get; }

        /// <summary>
        ///     Configure services on the serviceCollection, when called.
        /// </summary>
        /// <param name="serviceCollection">The serviceCollection.</param>
        void ConfigureServices(IServiceCollection serviceCollection);

        /// <summary>
        ///     Configures your services.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        void Configure(IServiceProvider serviceProvider);
    }
}