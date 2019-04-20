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

        /// <summary>
        ///     Request to join the <see cref="Team" /> for <see cref="TeamTunerUser" /> wit Id <see cref="userId" />
        /// </summary>
        /// <param name="userId">The Id of the user.</param>
        /// <param name="teamId">The Id of the team to join.</param>
        /// <param name="comment">A comment from the user.</param>
        Task RequestJoinAsync(Guid userId, Guid teamId, string comment);

        /// <summary>
        ///     Accepts the join request specified by <see cref="joinRequestId" />.
        /// </summary>
        /// <param name="joinRequestId">The join request identifier.</param>
        Task AcceptJoinAsync(Guid joinRequestId);

        /// <summary>
        ///     Refuses the join request specified by <see cref="joinRequestId" />.
        /// </summary>
        /// <param name="joinRequestId">The join request identifier.</param>
        Task RefuseJoinAsync(Guid joinRequestId);

        /// <summary>
        ///     Gets all open join requests for the team.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>A list of join requests.</returns>
        Task<IEnumerable<TeamJoinRequest>> GetJoinRequestsAsync(Guid teamId);

        /// <summary>
        ///     Gets the join request.
        /// </summary>
        /// <param name="joinRequestId">The join request identifier.</param>
        /// <returns>The join request.</returns>
        Task<TeamJoinRequest> GetJoinRequestAsync(Guid joinRequestId);
    }
}