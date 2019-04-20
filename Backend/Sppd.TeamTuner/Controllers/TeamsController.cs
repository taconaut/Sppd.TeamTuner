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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ITeamTunerUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITeamTunerUserProvider _userProvider;
        private readonly ITokenProvider _tokenProvider;
        private readonly IMapper _mapper;

        public TeamsController(ITeamService teamService, ITeamTunerUserService userService, IAuthorizationService authorizationService, ITeamTunerUserProvider userProvider,
            ITokenProvider tokenProvider, IMapper mapper)
        {
            _teamService = teamService;
            _userService = userService;
            _authorizationService = authorizationService;
            _userProvider = userProvider;
            _tokenProvider = tokenProvider;
            _mapper = mapper;
        }

        [HttpPut("create")]
        public async Task<IActionResult> Create([FromBody] TeamCreateRequestDto teamCreateRequestDto)
        {
            var teamToCreate = _mapper.Map<Team>(teamCreateRequestDto);
            var teamCreated = _teamService.CreateAsync(teamToCreate);
            var responseDto = _mapper.Map<TeamCreateResponseDto>(await teamCreated);
            responseDto.Token = _tokenProvider.GetToken(_userProvider.CurrentUser);
            return Ok();
        }

        [HttpPut("requestJoin")]
        public async Task<IActionResult> RequestJoin([FromBody] TeamJoinRequestDto joinRequest)
        {
            await _teamService.RequestJoinAsync(joinRequest.UserId, joinRequest.TeamId, joinRequest.Comment);
            return Ok();
        }

        [HttpPost("acceptJoin/{joinRequestId}")]
        public async Task<IActionResult> AcceptJoin(Guid joinRequestId)
        {
            var joinRequest = await _teamService.GetJoinRequestAsync(joinRequestId);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, joinRequest.TeamId, AuthorizationConstants.Policies.CAN_ACCEPT_TEAM_JOIN_REQUESTS);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _teamService.AcceptJoinAsync(joinRequestId);
            return Ok();
        }

        [HttpPost("refuseJoin/{joinRequestId}")]
        public async Task<IActionResult> RefuseJoin(Guid joinRequestId)
        {
            var joinRequest = await _teamService.GetJoinRequestAsync(joinRequestId);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, joinRequest.TeamId, AuthorizationConstants.Policies.CAN_ACCEPT_TEAM_JOIN_REQUESTS);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _teamService.RefuseJoinAsync(joinRequestId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, AuthorizationConstants.Policies.IS_ADMIN);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var teams = _teamService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TeamResponseDto>>(await teams));
        }

        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetById(Guid teamId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, teamId, AuthorizationConstants.Policies.IS_IN_TEAM);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var team = _teamService.GetByIdAsync(teamId);
            return Ok(_mapper.Map<TeamResponseDto>(await team));
        }

        [HttpGet("{teamId}/users")]
        public async Task<IActionResult> GetUsers(Guid teamId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, teamId, AuthorizationConstants.Policies.IS_IN_TEAM);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var users = _userService.GetByTeamIdAsync(teamId);
            return Ok(_mapper.Map<IEnumerable<UserResponseDto>>(await users));
        }

        [HttpGet("{teamId}/joinRequests")]
        public async Task<IActionResult> GetJoinRequests(Guid teamId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, teamId, AuthorizationConstants.Policies.CAN_ACCEPT_TEAM_JOIN_REQUESTS);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var joinRequests = _teamService.GetJoinRequestsAsync(teamId);
            return Ok(_mapper.Map<IEnumerable<TeamJoinRequestResponseDto>>(await joinRequests));
        }
    }
}