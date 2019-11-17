using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Repositories
{
    internal class TeamRepository : NamedEntityRepository<Team>, ITeamRepository
    {
        public TeamRepository(TeamTunerContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Team>> GetAllAsync(Guid federationId)
        {
            return await GetQueryableWithIncludes()
                         .Where(team => team.FederationId == federationId)
                         .ToListAsync();
        }
    }
}