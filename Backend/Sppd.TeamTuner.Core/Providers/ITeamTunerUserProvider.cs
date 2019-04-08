using Sppd.TeamTuner.Core.Domain.Interfaces;

namespace Sppd.TeamTuner.Core.Providers
{
    /// <summary>
    ///     Provides <see cref="ITeamTunerUser" />s.
    /// </summary>
    public interface ITeamTunerUserProvider
    {
        /// <summary>
        ///     Gets or sets the current user.
        /// </summary>
        ITeamTunerUser CurrentUser { get; set; }
    }
}