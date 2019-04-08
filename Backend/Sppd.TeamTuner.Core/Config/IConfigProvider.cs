namespace Sppd.TeamTuner.Core.Config
{
    /// <summary>
    ///     Defines a provider that delivers populated <see cref="IConfig" /> objects.
    /// </summary>
    public interface IConfigProvider<out TConfig>
        where TConfig : class, IConfig, new()
    {
        /// <summary>
        ///     Gets the configuration
        /// </summary>
        TConfig Config { get; }
    }
}