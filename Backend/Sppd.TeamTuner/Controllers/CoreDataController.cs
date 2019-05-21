using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.Controllers
{
    /// <summary>
    ///     Exposes an API to retrieve core data.
    /// </summary>
    /// <seealso cref="AuthorizationController" />
    [Authorize]
    [ApiController]
    [Route("core-data")]
    public class CoreDataController : AuthorizationController
    {
        private readonly ICoreDataService _coreDataService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CoreDataController" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="authorizationService">The authorization service.</param>
        /// <param name="coreDataService">The core data service.</param>
        /// <param name="mapper">The mapper.</param>
        public CoreDataController(IServiceProvider serviceProvider, IAuthorizationService authorizationService, ICoreDataService coreDataService, IMapper mapper)
            : base(serviceProvider, authorizationService)
        {
            _coreDataService = coreDataService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Gets all card types (Spell, Fighter...)
        /// </summary>
        [AllowAnonymous]
        [HttpGet("card-types")]
        public async Task<IActionResult> GetCardTypes()
        {
            var cardTypes = await _coreDataService.GetCardTypesAsync();
            return Ok(_mapper.Map<IEnumerable<CardTypeResponseDto>>(cardTypes));
        }

        /// <summary>
        ///     Gets all card types (Spell, Fighter...)
        /// </summary>
        [AllowAnonymous]
        [HttpGet("character-types")]
        public async Task<IActionResult> GetCharacterTypes()
        {
            var characterTypes = await _coreDataService.GetCharacterTypesAsync();
            return Ok(_mapper.Map<IEnumerable<CharacterTypeResponseDto>>(characterTypes));
        }

        /// <summary>
        ///     Gets all rarities (Common, Rare...)
        /// </summary>
        [AllowAnonymous]
        [HttpGet("rarities")]
        public async Task<IActionResult> GetRarities()
        {
            var rarities = await _coreDataService.GetRaritiesAsync();
            return Ok(_mapper.Map<IEnumerable<RarityResponseDto>>(rarities));
        }

        /// <summary>
        ///     Gets all themes (Sci-fy, Fantasy...)
        /// </summary>
        [AllowAnonymous]
        [HttpGet("themes")]
        public async Task<IActionResult> GetThemes()
        {
            var themes = await _coreDataService.GetThemesAsync();
            return Ok(_mapper.Map<IEnumerable<ThemeResponseDto>>(themes));
        }
    }
}