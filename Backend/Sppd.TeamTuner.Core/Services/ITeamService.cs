using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Services
{
    /// <summary>
    ///     Services for <see cref="Team" /> handling.
    /// </summary>
    /// <seealso cref="IServiceBase{Team}" />
    public interface ITeamService : IServiceBase<Team>
    {
        /// <summary>
        ///     Gets all teams in federation.
        /// </summary>
        /// <param name="federationId">The federation identifier.</param>
        /// <returns>A list of teams being part of the federation.</returns>
        Task<IEnumerable<Team>> GetAllAsync(Guid federationId);
    }
}