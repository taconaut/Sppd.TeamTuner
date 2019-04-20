using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Repositories
{
    public interface ITeamMembershipRequestRepository : IRepository<TeamMembershipRequest>
    {
        Task<IEnumerable<TeamMembershipRequest>> GetForTeam(Guid teamId);
    }
}