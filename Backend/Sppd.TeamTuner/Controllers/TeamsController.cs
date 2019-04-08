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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ITeamTunerUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public TeamsController(ITeamService teamService, ITeamTunerUserService userService, IAuthorizationService authorizationService, IMapper mapper)
        {
            _teamService = teamService;
            _userService = userService;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TeamCreateDto teamCreateDto)
        {
            var teamToCreate = _mapper.Map<Team>(teamCreateDto);
            var teamCreated = _teamService.CreateAsync(teamToCreate);
            return Ok(_mapper.Map<TeamDto>(await teamCreated));
        }

        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetByTeamId(Guid teamId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, teamId, AuthorizationConstants.Policies.IS_IN_TEAM);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var team = _teamService.GetByIdAsync(teamId);
            return Ok(_mapper.Map<TeamDto>(await team));
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
            return Ok(_mapper.Map<IEnumerable<TeamDto>>(await teams));
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
            return Ok(_mapper.Map<IEnumerable<UserDto>>(await users));
        }
    }
}