using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="SeedMode" />
    /// </summary>
    internal static class SeedModeExtensions
    {
        /// <summary>
        ///     Determines whether the specified <see cref="seedMode" /> requires to seed required data.
        /// </summary>
        public static bool MustSeedRequired(this SeedMode seedMode)
        {
            return seedMode == SeedMode.Required || seedMode == SeedMode.Test;
        }
    }
}