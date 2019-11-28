using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Repositories
{
    public interface INamedEntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : NamedEntity
    {
        /// <summary>
        /// Returns all <see cref="NamedEntity" /> containing <see cref="containName" /> in their name.
        /// </summary>
        /// <param name="containName">String having to be contained in the name.</param>
        /// <param name="includeProperties">The properties to include.</param>
        /// <param name="isCaseSensitive">if set to <c>true</c> [is case sensitive].</param>
        /// <returns>
        /// All <see cref="NamedEntity" /> containing <see cref="containName" /> in their name.
        /// </returns>
        Task<IEnumerable<TEntity>> SearchByNameAsync(string containName, IEnumerable<string> includeProperties = null, bool isCaseSensitive = false);
    }
}