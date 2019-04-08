using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Repositories
{
    internal class TeamRepository : Repository<Team>, ITeamRepository
    {
        public override Func<IQueryable<Team>, IQueryable<Team>> Includes => teams => teams.Include(team => team.Users);

        public TeamRepository(TeamTunerContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Team>> GetAllAsync(Guid federationId)
        {
            return await GetQueryWithIncludes().Where(team => team.FederationId == federationId).ToListAsync();
        }
    }
}