using Microsoft.Extensions.Logging;

namespace Sppd.TeamTuner.Core.Utils.Helpers
{
    /// <summary>Offers helper methods for logging</summary>
    public class LoggerHelper
    {
        /// <summary>
        ///     Gets or sets the logger factory.
        /// </summary>
        /// <value>
        ///     The logger factory.
        /// </value>
        public static ILoggerFactory LoggerFactory { get; set; }

        /// <summary>
        ///     Creates a new <see cref="ILogger{TCategoryName}" /> instance using the full name of the given type <see cref="T" />
        ///     .
        /// </summary>
        /// <typeparam name="T">The generic type specifying the full name that will be used to create the logger.</typeparam>
        /// <returns>An <see cref="ILogger{TCategoryName}" /> instance.</returns>
        public static ILogger<T> CreateLogger<T>()
        {
            return LoggerFactory.CreateLogger<T>();
        }

        /// <summary>
        ///     Creates a new <see cref="ILogger" />  instance.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>An <see cref="ILogger" /> instance.</returns>
        public static ILogger CreateLogger(string categoryName)
        {
            return LoggerFactory.CreateLogger(categoryName);
        }
    }
}