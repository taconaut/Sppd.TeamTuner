using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Repositories
{
    /// <summary>
    ///     Repository to handle <see cref="TeamTunerUser" />s
    /// </summary>
    /// <seealso cref="Repositories.IRepository{TeamTunerUser}" />
    public interface ITeamTunerUserRepository : IRepository<TeamTunerUser>
    {
        /// <summary>
        ///     Gets the <see cref="TeamTunerUser" /> having the specified <see cref="username" />
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The <see cref="TeamTunerUser" /> if it could be found; otherwise null.</returns>
        Task<TeamTunerUser> GetByNameAsync(string username);

        /// <summary>
        ///     Gets all <see cref="TeamTunerUser" />s being part of the <see cref="Team" />
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>The list of <see cref="TeamTunerUser" /> being part of the team.</returns>
        Task<IEnumerable<TeamTunerUser>> GetByTeamIdAsync(Guid teamId);
    }
}