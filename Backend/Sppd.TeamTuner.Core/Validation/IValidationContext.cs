using System;

namespace Sppd.TeamTuner.Core.Validation
{
    /// <summary>
    ///     Validation context
    /// </summary>
    public interface IValidationContext
    {
        /// <summary>
        ///     Gets the service provider which can be used to resolve services.
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }
}