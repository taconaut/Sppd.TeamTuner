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
    ///     Exposes an API to manage card levels.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    public class CardLevelsController : ControllerBase
    {
        private readonly ITeamTunerUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CardLevelsController" /> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="authorizationService">The authorization service.</param>
        /// <param name="mapper">The mapper.</param>
        public CardLevelsController(ITeamTunerUserService userService, IAuthorizationService authorizationService, IMapper mapper)
        {
            _userService = userService;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Sets the card level for the given user and card
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> SetCardLevel([FromBody] SetCardLevelRequestDto cardLevelDto)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, cardLevelDto.UserId, AuthorizationConstants.Policies.IS_OWNER);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var cardLevel = await _userService.SetCardLevelAsync(_mapper.Map<CardLevel>(cardLevelDto));
            return Ok(_mapper.Map<CardLevelResponseDto>(cardLevel));
        }
    }
}