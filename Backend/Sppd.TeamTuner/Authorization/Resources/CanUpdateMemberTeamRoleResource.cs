using System;

namespace Sppd.TeamTuner.Authorization.Resources
{
    internal class CanUpdateMemberTeamRoleResource
    {
        public Guid TeamId { get; set; }

        public Guid UserId { get; set; }

        public string Role { get; set; }
    }
}