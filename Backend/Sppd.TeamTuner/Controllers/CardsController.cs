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
    ///     Exposes an API to manage cards.
    /// </summary>
    /// <seealso cref="AuthorizationController" />
    [Authorize]
    [ApiController]
    [Route("cards")]
    public class CardsController : AuthorizationController
    {
        private readonly ICardService _cardService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CardsController" /> class.
        /// </summary>
        /// <param name="cardService">The card service.</param>
        /// <param name="authorizationService">The authorization service.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mapper">The mapper.</param>
        public CardsController(ICardService cardService, IAuthorizationService authorizationService, IServiceProvider serviceProvider, IMapper mapper)
            : base(serviceProvider, authorizationService)
        {
            _cardService = cardService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Gets all the cards
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cards = await _cardService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CardResponseDto>>(cards));
        }
    }
}