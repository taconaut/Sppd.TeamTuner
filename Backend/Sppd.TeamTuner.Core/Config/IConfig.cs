namespace Sppd.TeamTuner.Core.Config
{

    /// <summary>
    /// Represents a configuration
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// Specifies the section key which will be used to load the configuration.
        /// </summary>
        string SectionKey { get; }
    }
}