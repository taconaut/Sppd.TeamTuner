using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Providers;
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
        private readonly ITeamTunerUserProvider _userProvider;
        private readonly ITokenProvider _tokenProvider;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TeamsController" /> class.
        /// </summary>
        /// <param name="teamService">The team service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="authorizationService">The authorization service.</param>
        /// <param name="userProvider">The user provider.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="mapper">The mapper.</param>
        public TeamsController(ITeamService teamService, ITeamTunerUserService userService, IAuthorizationService authorizationService, ITeamTunerUserProvider userProvider,
            ITokenProvider tokenProvider, IMapper mapper)
            : base(userProvider, authorizationService)
        {
            _teamService = teamService;
            _userService = userService;
            _userProvider = userProvider;
            _tokenProvider = tokenProvider;
            _mapper = mapper;
        }

        /// <summary>
        ///     Creates a new team
        /// </summary>
        /// <param name="teamCreateRequestDto">The team create request dto.</param>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeamCreateRequestDto teamCreateRequestDto)
        {
            var teamToCreate = _mapper.Map<Team>(teamCreateRequestDto);
            var teamCreated = await _teamService.CreateAsync(teamToCreate);
            var responseDto = _mapper.Map<TeamCreateResponseDto>(teamCreated);
            responseDto.Token = _tokenProvider.GetToken(_userProvider.CurrentUser);
            return Ok();
        }

        /// <summary>
        ///     Updates the team
        /// </summary>
        /// <param name="teamUpdateRequestDto">The team update request dto.</param>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TeamUpdateRequestDto teamUpdateRequestDto)
        {
            // TODO: authorize

            var team = _mapper.Map<Team>(teamUpdateRequestDto);
            var teamCreated = await _teamService.UpdateAsync(team, teamUpdateRequestDto.PropertiesToUpdate);
            return Ok(_mapper.Map<TeamCreateResponseDto>(teamCreated));
        }

        /// <summary>
        ///     Deletes the team
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            // TODO: authorize

            await _teamService.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        ///     Gets all teams
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.IS_ADMIN, null);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var teams = await _teamService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TeamResponseDto>>(teams));
        }

        /// <summary>
        ///     Gets the team
        /// </summary>
        /// <param name="id">The team identifier.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.IS_IN_TEAM, id);
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
        /// <param name="id">The team identifier.</param>
        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetUsers(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.IS_IN_TEAM, id);
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
        /// <param name="id">The team identifier.</param>
        [HttpGet("{id}/membership-requests")]
        public async Task<IActionResult> GetMembershipRequests(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_ACCEPT_TEAM_MEMBERSHIP_REQUESTS, id);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var joinRequests = await _teamService.GetMembershipRequestsAsync(id);
            return Ok(_mapper.Map<IEnumerable<TeamMembershipRequestResponseDto>>(joinRequests));
        }
    }
}