using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.Controllers
{
    /// <summary>
    ///     Exposes an API to manage team membership requests.
    /// </summary>
    /// <seealso cref="AuthorizationController" />
    [Authorize]
    [ApiController]
    [Route("team-membership-requests")]
    public class TeamMembershipRequestsController : AuthorizationController
    {
        private readonly ITeamService _teamService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TeamMembershipRequestsController" /> class.
        /// </summary>
        /// <param name="teamService">The team service.</param>
        /// <param name="authorizationService">The authorization service.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public TeamMembershipRequestsController(ITeamService teamService, IAuthorizationService authorizationService, IServiceProvider serviceProvider)
            : base(serviceProvider, authorizationService)
        {
            _teamService = teamService;
        }

        /// <summary>
        ///     Requests membership in a team for a user
        /// </summary>
        /// <param name="membershipRequest">The membership request for a user to join a team</param>
        [HttpPost]
        public async Task<IActionResult> RequestMembership([FromBody] TeamMembershipRequestDto membershipRequest)
        {
            await _teamService.RequestMembershipAsync(membershipRequest.UserId, membershipRequest.TeamId, membershipRequest.Comment);
            return Ok();
        }

        /// <summary>
        ///     Accepts the membership request
        /// </summary>
        /// <param name="id">The membership request identifier</param>
        [HttpPost("{id}/accept")]
        public async Task<IActionResult> AcceptMembershipRequest(Guid id)
        {
            var membershipRequest = await _teamService.GetMembershipRequestAsync(id);

            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_ACCEPT_TEAM_MEMBERSHIP_REQUESTS, membershipRequest.TeamId);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _teamService.AcceptMembershipAsync(id);
            return Ok();
        }

        /// <summary>
        ///     Rejects the membership request
        /// </summary>
        /// <param name="id">The membership request identifier</param>
        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectMembershipRequest(Guid id)
        {
            var membershipRequest = await _teamService.GetMembershipRequestAsync(id);

            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_ACCEPT_TEAM_MEMBERSHIP_REQUESTS, membershipRequest.TeamId);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _teamService.RejectMembershipAsync(id);
            return Ok();
        }
    }
}