using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sppd.TeamTuner.Authorization;
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
        public async Task<ActionResult<TeamResponseDto>> CreateTeam([FromBody] TeamCreateRequestDto teamCreateRequestDto)
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
        public async Task<ActionResult<TeamResponseDto>> UpdateTeam([FromBody] TeamUpdateRequestDto teamUpdateRequestDto)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_UPDATE_TEAM, teamUpdateRequestDto.Id);
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
        public async Task<IActionResult> DeleteTeam(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_DELETE_TEAM, id);
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
        public async Task<ActionResult<TeamResponseDto>> GetTeamById(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_READ_TEAM, id);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var team = await _teamService.GetByIdAsync(id);
            return Ok(_mapper.Map<TeamResponseDto>(team));
        }

        /// <summary>
        ///     Gets the team users
        /// </summary>
        /// <param name="id">The team identifier</param>
        [HttpGet("{id}/users")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetTeamUsers(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_READ_TEAM, id);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var users = await _userService.GetByTeamIdAsync(id);
            return Ok(_mapper.Map<IEnumerable<UserResponseDto>>(users));
        }

        /// <summary>
        ///     Gets the membership requests.
        /// </summary>
        /// <param name="id">The team identifier</param>
        [HttpGet("{id}/membership-requests")]
        public async Task<ActionResult<IEnumerable<TeamMembershipRequestResponseDto>>> GetTeamMembershipRequests(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_ACCEPT_TEAM_MEMBERSHIP_REQUESTS, id);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var membershipRequests = await _teamService.GetMembershipRequestsAsync(id);
            return Ok(_mapper.Map<IEnumerable<TeamMembershipRequestResponseDto>>(membershipRequests));
        }
    }
}