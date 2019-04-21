using Sppd.TeamTuner.Core.Config;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config
{
    /// <summary>
    ///     Defines configuration properties related to the database
    /// </summary>
    /// <seealso cref="IConfig" />
    internal class DatabaseConfig : IConfig
    {
        /// <summary>
        ///     ConnectionString used for database connection.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     If set to true, the database will be automatically migrated and created when required.
        /// </summary>
        public bool AutoMigrate { get; set; }

        /// <summary>
        ///     If set to true, the database will be automatically migrated when required.
        /// </summary>
        public SeedMode SeedMode { get; set; }

        /// <summary>
        ///     If set to true, the database will be deleted automatically when the application starts
        /// </summary>
        public bool DeleteOnStartup { get; set; }

        public string SectionKey => "Database";
    }
}