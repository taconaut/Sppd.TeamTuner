﻿using System;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Authorization.Resources;
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
        private readonly ITeamTunerUserService _userService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TeamMembershipRequestsController" /> class.
        /// </summary>
        /// <param name="teamService">The team service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="authorizationService">The authorization service.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mapper">The mapper.</param>
        public TeamMembershipRequestsController(ITeamService teamService, ITeamTunerUserService userService, IAuthorizationService authorizationService,
            IServiceProvider serviceProvider, IMapper mapper)
            : base(serviceProvider, authorizationService)
        {
            _teamService = teamService;
            _userService = userService;
            _mapper = mapper;
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
        ///     Gets the pending team membership request for the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The <see cref="TeamMembershipRequestDto" /> if it could be found; otherwise <c>NULL</c>.</returns>
        [HttpGet]
        public async Task<ActionResult<TeamMembershipRequestResponseDto>> GetPendingTeamMembershipRequest([FromQuery] Guid userId)
        {
            var request = await _userService.GetPendingTeamMembershipRequest(userId);
            var response = request == null
                ? null
                : _mapper.Map<TeamMembershipRequestResponseDto>(request);

            return Ok(response);
        }

        /// <summary>
        ///     Accepts the membership request
        /// </summary>
        /// <param name="id">The membership request identifier</param>
        [HttpPut("{id}/accept")]
        public async Task<IActionResult> AcceptMembershipRequest(Guid id)
        {
            var membershipRequest = await _teamService.GetMembershipRequestAsync(id);

            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_MANAGE_TEAM_MEMBERSHIP_REQUESTS,
                new CanManageTeamMembershipRequestsResource {TeamId = membershipRequest.TeamId});
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
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectMembershipRequest(Guid id)
        {
            var membershipRequest = await _teamService.GetMembershipRequestAsync(id);

            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_MANAGE_TEAM_MEMBERSHIP_REQUESTS,
                new CanManageTeamMembershipRequestsResource {TeamId = membershipRequest.TeamId});
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _teamService.RejectMembershipAsync(id);
            return Ok();
        }

        /// <summary>
        ///     Aborts the membership request
        /// </summary>
        /// <param name="id">The membership request identifier</param>
        [HttpPut("{id}/abort")]
        public async Task<IActionResult> AbortMembershipRequest(Guid id)
        {
            var membershipRequest = await _teamService.GetMembershipRequestAsync(id);

            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_ABORT_TEAM_MEMBERSHIP_REQUESTS,
                new CanAbortTeamMembershipResource {UserId = membershipRequest.UserId});
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _teamService.AbortMembershipRequestAsync(id);
            return Ok();
        }
    }
}