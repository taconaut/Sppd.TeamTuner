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
        ///     Specifies if hangfire will be enabled
        /// </summary>
        public bool EnableHangfire { get; set; }

        public string SectionKey => "General";
    }
}