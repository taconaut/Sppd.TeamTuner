using Sppd.TeamTuner.Core.Domain.Enumerations;

namespace Sppd.TeamTuner.Core.Config
{
    /// <summary>
    ///     General configuration.
    /// </summary>
    /// <seealso cref="IConfig" />
    public class GeneralConfig : IConfig
    {
        /// <summary>
        ///     Specifies if Swagger UI will be enabled
        /// </summary>
        public bool EnableSwaggerUI { get; set; }

        /// <summary>
        ///     Specifies in which mode the application is being executed
        /// </summary>
        public ExecutionMode ExecutionMode { get; set; }

        public string SectionKey => "General";
    }
}