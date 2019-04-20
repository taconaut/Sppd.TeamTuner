using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Repositories
{
    public interface ITeamJoinRequestRepository : IRepository<TeamJoinRequest>
    {
        Task<IEnumerable<TeamJoinRequest>> GetForTeam(Guid teamId);
    }
}