namespace Sppd.TeamTuner.Core.Config
{
    /// <summary>
    ///     Authorization configuration.
    /// </summary>
    /// <seealso cref="IConfig" />
    public class AuthConfig : IConfig
    {
        /// <summary>
        ///     Secret used to generate the salt when storing the password
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        ///     Specifies the number of days the HTTP bearer token remains valid
        /// </summary>
        public int TokenExpirationDays { get; set; } = 7;

        public string SectionKey => "Auth";
    }
}