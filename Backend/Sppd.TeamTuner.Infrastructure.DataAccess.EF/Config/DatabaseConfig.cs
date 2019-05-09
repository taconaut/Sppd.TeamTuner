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
        ///     If set to true, the database will be automatically managed; otherwise the database will remain fully untouched.
        /// </summary>
        public bool ManageDatabaseSchema { get; set; }

        /// <summary>
        /// If set true, the database will be created if it doesn't exist and updated to the latest migration.
        /// </summary>
        public bool Initialize { get; set; } = true;

        /// <summary>
        ///     If set to true, the database will be automatically migrated when required.
        /// </summary>
        public SeedMode SeedMode { get; set; }

        /// <summary>
        ///     ConnectionString used for database connection.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     If set to true, the database will be deleted automatically when the application starts
        /// </summary>
        public bool DeleteOnStartup { get; set; }

        public string SectionKey => "Database";
    }
}