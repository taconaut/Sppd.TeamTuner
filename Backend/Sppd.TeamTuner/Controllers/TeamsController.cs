using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Authorization.Resources;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.Controllers
{
    /// <summary>
    ///     Exposes an API to manage teams.
    /// </summary>
    /// <seealso cref="AuthorizationController" />
    [Authorize]
    [ApiController]
    [Route("teams")]
    public class TeamsController : AuthorizationController
    {
        private readonly ITeamService _teamService;
        private readonly ITeamTunerUserService _userService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TeamsController" /> class.
        /// </summary>
        /// <param name="teamService">The team service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="authorizationService">The authorization service.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mapper">The mapper.</param>
        public TeamsController(ITeamService teamService, ITeamTunerUserService userService, IAuthorizationService authorizationService, IServiceProvider serviceProvider,
            IMapper mapper)
            : base(serviceProvider, authorizationService)
        {
            _teamService = teamService;
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Creates a new team
        /// </summary>
        /// <param name="teamCreateRequestDto">The team creation request</param>
        [HttpPost]
        public async Task<ActionResult<TeamResponseDto>> CreateTeam([Required] [FromBody] TeamCreateRequestDto teamCreateRequestDto)
        {
            var team = _mapper.Map<Team>(teamCreateRequestDto);
            await _teamService.CreateAsync(team);
            var responseDto = _mapper.Map<TeamResponseDto>(team);
            return Ok(responseDto);
        }

        /// <summary>
        ///     Updates the team
        /// </summary>
        /// <param name="teamUpdateRequestDto">The team update request</param>
        [HttpPut]
        public async Task<ActionResult<TeamResponseDto>> UpdateTeam([Required] [FromBody] TeamUpdateRequestDto teamUpdateRequestDto)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_UPDATE_TEAM, new CanUpdateTeamResource {TeamId = teamUpdateRequestDto.Id});
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var team = _mapper.Map<Team>(teamUpdateRequestDto);
            var teamCreated = await _teamService.UpdateAsync(team, teamUpdateRequestDto.PropertiesToUpdate);
            return Ok(_mapper.Map<TeamResponseDto>(teamCreated));
        }

        /// <summary>
        ///     Deletes the team
        /// </summary>
        /// <param name="id">The team identifier</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam([Required] Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_DELETE_TEAM, new CanDeleteTeamResource {TeamId = id});
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _teamService.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        ///     Gets the team
        /// </summary>
        /// <param name="id">The team identifier</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamResponseDto>> GetTeamById([Required] Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_READ_TEAM, new CanReadTeamResource {TeamId = id});
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var team = await _teamService.GetByIdAsync(id);
            return Ok(_mapper.Map<TeamResponseDto>(team));
        }

        /// <summary>
        ///     Get all teams containing the specified name in their name.
        /// </summary>
        /// <param name="name">The name having to be contained in the team name.</param>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamResponseDto>>> SearchTeamByName([Required] [FromQuery] string name)
        {
            var teams = await _teamService.SearchByNameAsync(name);
            return Ok(_mapper.Map<IEnumerable<TeamResponseDto>>(teams));
        }

        /// <summary>
        ///     Gets the team members.
        /// </summary>
        /// <param name="id">The team identifier</param>
        [HttpGet("{id}/members")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetTeamMembers([Required] Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_READ_TEAM, new CanReadTeamResource {TeamId = id});
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var users = await _userService.GetByTeamIdAsync(id);
            return Ok(_mapper.Map<IEnumerable<UserResponseDto>>(users));
        }

        /// <summary>
        ///     Gets the team membership requests.
        /// </summary>
        /// <param name="id">The team identifier</param>
        [HttpGet("{id}/membership-requests")]
        public async Task<ActionResult<IEnumerable<TeamMembershipRequestResponseDto>>> GetTeamMembershipRequests([Required] Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_MANAGE_TEAM_MEMBERSHIP_REQUESTS,
                new CanManageTeamMembershipRequestsResource {TeamId = id});
            if (!authorizationResult.Succeeded)
            {
                var tt = authorizationResult.Failure.FailedRequirements;
                return Forbid();
            }

            var membershipRequests = await _teamService.GetMembershipRequestsAsync(id);
            return Ok(_mapper.Map<IEnumerable<TeamMembershipRequestResponseDto>>(membershipRequests));
        }

        /// <summary>
        ///     Updates the team role for the user.
        /// </summary>
        /// <param name="id">The team identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="role">The role.</param>
        /// <returns><see cref="OkResult" /> if the member could be removed.</returns>
        [HttpPut("{id}/members/{userId}/role")]
        public async Task<ActionResult<string>> UpdateMemberTeamRole([Required] Guid id, [Required] Guid userId, [Required] [FromBody] string role)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_UPDATE_USER_TEAM_ROLE, new CanUpdateMemberTeamRoleResource
                                                                                                                      {
                                                                                                                          TeamId = id,
                                                                                                                          UserId = userId,
                                                                                                                          Role = role
                                                                                                                      });
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _userService.UpdateTeamRoleAsync(userId, role);

            return Ok();
        }

        /// <summary>
        ///     Removes the member from the team.
        /// </summary>
        /// <param name="id">The team identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns><see cref="OkResult" /> if the member could be removed.</returns>
        [HttpPut("{id}/members/{userId}/remove")]
        public async Task<IActionResult> RemoveMember([Required] Guid id, [Required] Guid userId)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER, new CanRemoveTeamMemberResource {TeamId = id, UserId = userId});
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            // It is not necessary to pass the team id to the service as it has been validated during authorization
            // that the user is being removed from the team he is currently part of.
            await _userService.LeaveTeam(userId);

            return Ok();
        }
    }
}