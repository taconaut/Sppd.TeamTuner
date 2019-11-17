using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Repositories
{
    /// <summary>
    ///     Repository to handle <see cref="Team" />s
    /// </summary>
    /// <seealso cref="IRepository{Team}" />
    public interface ITeamRepository : INamedEntityRepository<Team>
    {
        /// <summary>
        ///     Gets all teams being part of the federation.
        /// </summary>
        /// <param name="federationId">The federation identifier.</param>
        /// <returns>A list of all teams in the federation.</returns>
        Task<IEnumerable<Team>> GetAllAsync(Guid federationId);
    }
}