using Sppd.TeamTuner.Core.Config;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config
{
    /// <summary>
    ///     Defines configuration properties related to the database
    /// </summary>
    /// <seealso cref="IConfig" />
    public class DatabaseConfig : IConfig
    {
        /// <summary>
        ///     DB provider to use.
        /// </summary>
        public string Provider { get; set; } = "MsSql";

        /// <summary>
        ///     ConnectionString used for database connection.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     If set to true, the database will be automatically migrated and created when required.
        /// </summary>
        public bool AutoMigrate { get; set; }

        /// <summary>
        /// If set true, the database will be initialized (the database will be migrated)
        /// </summary>
        public bool Initialize { get; set; } = true;

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