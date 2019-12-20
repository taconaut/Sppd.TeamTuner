using System;

namespace Sppd.TeamTuner.Authorization.Resources
{
    internal class CanRemoveTeamMemberResource
    {
        public Guid TeamId { get; set; }

        public Guid UserId { get; set; }
    }
}