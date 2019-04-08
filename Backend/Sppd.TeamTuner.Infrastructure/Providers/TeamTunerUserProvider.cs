using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Providers;

namespace Sppd.TeamTuner.Infrastructure.Providers
{
    internal class TeamTunerUserProvider : ITeamTunerUserProvider
    {
        public ITeamTunerUser CurrentUser { get; set; }
    }
}