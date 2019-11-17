using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Repositories
{
    public interface ITeamMembershipRequestRepository : IRepository<TeamMembershipRequest>
    {
        /// <summary>
        /// Gets all open requests for the team.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="includeProperties">The properties to include.</param>
        /// <returns>
        /// A list of all active <see cref="TeamMembershipRequest" />
        /// </returns>
        Task<IEnumerable<TeamMembershipRequest>> GetForTeam(Guid teamId, IEnumerable<string> includeProperties = null);

        /// <summary>
        /// Gets a single <see cref="TeamMembershipRequest" /> for the user, if it exists
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="includeProperties">The properties to include.</param>
        /// <returns>
        /// The single <see cref="TeamMembershipRequest" /> for the user, if it exists; otherwise <c>NULL</c>
        /// </returns>
        Task<TeamMembershipRequest> GetForUser(Guid userId, IEnumerable<string> includeProperties = null);
    }
}