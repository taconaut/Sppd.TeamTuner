﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Repositories
{
    internal class TeamMembershipRequestRepository : Repository<TeamMembershipRequest>, ITeamMembershipRequestRepository
    {
        public TeamMembershipRequestRepository(TeamTunerContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<TeamMembershipRequest>> GetForTeam(Guid teamId, IEnumerable<string> includeProperties = null)
        {
            return await GetQueryableWithIncludes(includeProperties)
                         .Where(e => e.TeamId == teamId)
                         .ToListAsync();
        }

        public async Task<TeamMembershipRequest> GetForUser(Guid userId, IEnumerable<string> includeProperties = null)
        {
            return await GetQueryableWithIncludes(includeProperties).SingleOrDefaultAsync(e => e.UserId == userId);
        }
    }
}