using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Repositories
{
    internal class TeamJoinRequestRepository : Repository<TeamJoinRequest>, ITeamJoinRequestRepository
    {
        public TeamJoinRequestRepository(TeamTunerContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TeamJoinRequest>> GetForTeam(Guid teamId)
        {
            return await GetQueryableWithIncludes()
                         .Include(e => e.User)
                         .Where(e => e.TeamId == teamId)
                         .ToListAsync();
        }
    }
}