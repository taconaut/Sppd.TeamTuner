using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Objects;

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
        ///     get all teams having a name containing the specified <see cref="teamName" />
        /// </summary>
        /// <param name="teamName">Name of the team.</param>
        /// <returns>A list of teams having a name containing <see cref="teamName" />.</returns>
        Task<IEnumerable<Team>> SearchByNameAsync(string teamName);

        /// <summary>
        ///     Request to join the <see cref="Team" /> for <see cref="TeamTunerUser" /> wit Id <see cref="userId" />
        /// </summary>
        /// <param name="userId">The Id of the user.</param>
        /// <param name="teamId">The Id of the team to join.</param>
        /// <param name="comment">A comment from the user.</param>
        Task RequestMembershipAsync(Guid userId, Guid teamId, string comment);

        /// <summary>
        ///     Accepts the membership request specified by <see cref="membershipRequestId" />.
        /// </summary>
        /// <param name="membershipRequestId">The membership request identifier.</param>
        /// <returns></returns>
        Task AcceptMembershipAsync(Guid membershipRequestId);

        /// <summary>
        ///     Refuses the membership request specified by <see cref="membershipRequestId" />.
        /// </summary>
        /// <param name="membershipRequestId">The membership request identifier.</param>
        Task RejectMembershipAsync(Guid membershipRequestId);

        /// <summary>
        ///     Gets all open membership requests for the team.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>A list of membership requests.</returns>
        Task<IEnumerable<TeamMembershipRequest>> GetMembershipRequestsAsync(Guid teamId);

        /// <summary>
        ///     Gets the membership request.
        /// </summary>
        /// <param name="membershipRequestId">The membership request identifier.</param>
        /// <returns>The membership request.</returns>
        Task<TeamMembershipRequest> GetMembershipRequestAsync(Guid membershipRequestId);

        /// <summary>
        ///     Aborts the membership request.
        /// </summary>
        /// <param name="membershipRequestId">The membership request identifier.</param>
        Task AbortMembershipRequestAsync(Guid membershipRequestId);

        /// <summary>
        ///     Gets all cards including the levels set by the users.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>All cards including the levels set by the users.</returns>
        Task<TeamCards> GetCardsAsync(Guid teamId);
    }
}