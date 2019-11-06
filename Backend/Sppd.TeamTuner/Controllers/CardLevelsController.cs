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
    /// <summary>
    ///     Exposes an API to manage card levels.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Authorize]
    [ApiController]
    [Route("card-levels")]
    public class CardLevelsController : AuthorizationController
    {
        private readonly ITeamTunerUserService _userService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CardLevelsController" /> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="authorizationService">The authorization service.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mapper">The mapper.</param>
        public CardLevelsController(ITeamTunerUserService userService, IAuthorizationService authorizationService, IServiceProvider serviceProvider, IMapper mapper)
            : base(serviceProvider, authorizationService)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Sets the card level for the given user and card
        /// </summary>
        /// <param name="cardLevelUpdateDto">The card level update</param>
        [HttpPut]
        public async Task<ActionResult<CardLevelResponseDto>> SetCardLevel([FromBody] CardLevelUpdateRequestDto cardLevelUpdateDto)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_UPDATE_USER, cardLevelUpdateDto.UserId);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var cardLevel = await _userService.SetCardLevelAsync(_mapper.Map<CardLevel>(cardLevelUpdateDto));
            return Ok(_mapper.Map<CardLevelResponseDto>(cardLevel));
        }
    }
}