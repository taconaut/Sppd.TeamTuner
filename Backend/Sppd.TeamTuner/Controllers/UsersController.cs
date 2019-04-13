using System;
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
    public class UsersController : ControllerBase
    {
        private readonly ITeamTunerUserService _userService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public UsersController(ITeamTunerUserService userService, ITokenProvider tokenProvider, IAuthorizationService authorizationService, IMapper mapper)
        {
            _userService = userService;
            _tokenProvider = tokenProvider;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto userLoginRequestDto)
        {
            var user = _userService.AuthenticateAsync(userLoginRequestDto.Name, userLoginRequestDto.PasswordMd5);
            var userDto = _mapper.Map<UserLoginResponseDto>(await user);
            userDto.Token = _tokenProvider.GetToken(await user);
            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateRequestDto userCreateRequestDto)
        {
            var userToCreate = _mapper.Map<TeamTunerUser>(userCreateRequestDto);
            var createdUser = _userService.CreateAsync(userToCreate, userCreateRequestDto.PasswordMd5);
            return Ok(_mapper.Map<UserResponseDto>(await createdUser));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, userId, AuthorizationConstants.Policies.IS_OWNER);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var user = _userService.GetByIdAsync(userId);
            return Ok(_mapper.Map<UserResponseDto>(await user));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequestDto userRequestDto)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, userRequestDto.Id, AuthorizationConstants.Policies.IS_OWNER);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var user = _mapper.Map<TeamTunerUser>(userRequestDto);
            var updatedUser = _userService.UpdateAsync(user, userRequestDto.PropertiesToUpdate);
            return Ok(_mapper.Map<UserResponseDto>(await updatedUser));
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, userId, AuthorizationConstants.Policies.IS_OWNER);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _userService.DeleteAsync(userId);
            return Ok();
        }
    }
}