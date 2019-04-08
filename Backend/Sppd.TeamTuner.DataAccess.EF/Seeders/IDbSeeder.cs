using System.Threading.Tasks;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Seeders
{
    /// <summary>
    ///     Interface having to be implemented to seed data for an entity.
    /// </summary>
    internal interface IDbSeeder
    {
        /// <summary>
        ///     Determines the order in which seeders will get called.
        ///     Seeder with a lower priority will be seeded first.
        /// </summary>
        int Priority { get; }

        /// <summary>
        ///     Seeds the data.
        /// </summary>
        Task SeedAsync();
    }
}