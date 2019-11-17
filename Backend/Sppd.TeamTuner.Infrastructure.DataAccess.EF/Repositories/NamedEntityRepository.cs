using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Repositories
{
    internal class NamedEntityRepository<TEntity> : Repository<TEntity>, INamedEntityRepository<TEntity>
        where TEntity : NamedEntity
    {
        public NamedEntityRepository(TeamTunerContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<TEntity>> SearchByNameAsync(string containName, IEnumerable<string> includeProperties = null)
        {
            return await GetQueryableWithIncludes(includeProperties).Where(e => e.Name.Contains(containName))
                                                                    .ToListAsync();
        }
    }
}