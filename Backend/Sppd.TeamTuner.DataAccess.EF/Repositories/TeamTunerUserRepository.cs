using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Repositories
{
    internal class TeamTunerUserRepository : Repository<TeamTunerUser>, ITeamTunerUserRepository
    {
        public TeamTunerUserRepository(TeamTunerContext context)
            : base(context)
        {
        }

        public async Task<TeamTunerUser> GetByNameAsync(string name)
        {
            return await GetQueryableWithIncludes()
                .SingleOrDefaultAsync(e => e.Name == name);
        }

        public async Task<IEnumerable<TeamTunerUser>> GetByTeamIdAsync(Guid teamId)
        {
            return await GetQueryableWithIncludes()
                         .Where(e => e.TeamId == teamId)
                         .ToListAsync();
        }
    }
}