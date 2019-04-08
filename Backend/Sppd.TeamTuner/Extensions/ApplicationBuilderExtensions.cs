using Microsoft.AspNetCore.Builder;

using Sppd.TeamTuner.Middlewares;

namespace Sppd.TeamTuner.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="IApplicationBuilder" />
    /// </summary>
    internal static class ApplicationBuilderExtensions
    {
        /// <summary>
        ///     Registers the global exception handler global exception handler.
        /// </summary>
        public static IApplicationBuilder UseGlobalExceptionLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionLoggerMiddleware>();
        }

        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}