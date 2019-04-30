using System;
using System.Collections.Generic;
using System.Linq;
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
    ///     Exposes an API to manage users.
    /// </summary>
    /// <seealso cref="AuthorizationController" />
    [Authorize]
    [ApiController]
    [Route("users")]
    public class UsersController : AuthorizationController
    {
        private readonly ITeamTunerUserService _userService;
        private readonly ICardService _cardService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UsersController" /> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="cardService">The card service.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="authorizationService">The authorization service.</param>
        /// <param name="userProvider">The token provider.</param>
        /// <param name="mapper">The mapper.</param>
        public UsersController(ITeamTunerUserService userService, ICardService cardService, ITokenProvider tokenProvider, IAuthorizationService authorizationService,
            ITeamTunerUserProvider userProvider, IMapper mapper)
            : base(userProvider, authorizationService)
        {
            _userService = userService;
            _cardService = cardService;
            _tokenProvider = tokenProvider;
            _mapper = mapper;
        }

        /// <summary>
        ///     Creates a new user
        /// </summary>
        /// <remarks>
        ///     As stated by its name, the client has to send the MD5 hash of the password (length=36). This MD5 hash will be
        ///     stored as salted hash in the DB.
        /// </remarks>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserCreateRequestDto userCreateRequestDto)
        {
            var userToCreate = _mapper.Map<TeamTunerUser>(userCreateRequestDto);
            var createdUser = await _userService.CreateAsync(userToCreate, userCreateRequestDto.PasswordMd5);
            return Ok(_mapper.Map<UserResponseDto>(createdUser));
        }

        /// <summary>
        ///     Updates the user
        /// </summary>
        /// <remarks>
        ///     If the PropertiesToUpdate have been specified, only these will be updated; otherwise, all properties will be
        ///     updated.
        /// </remarks>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequestDto userRequestDto)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.IS_OWNER, userRequestDto.Id);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var user = _mapper.Map<TeamTunerUser>(userRequestDto);
            var updatedUser = await _userService.UpdateAsync(user, userRequestDto.PropertiesToUpdate);
            return Ok(_mapper.Map<UserResponseDto>(updatedUser));
        }

        /// <summary>
        ///     Authorizes the user
        /// </summary>
        /// <remarks>
        ///     The response contains a token which has to be included as bearer token in the HTTP authorization header for
        ///     subsequent calls to API methods requiring authentication.
        /// </remarks>
        [AllowAnonymous]
        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize([FromBody] AuthorizationRequestDto authorizationRequestDto)
        {
            var user = await _userService.AuthenticateAsync(authorizationRequestDto.Name, authorizationRequestDto.PasswordMd5);
            var userDto = _mapper.Map<UserAuthorizationResponseDto>(user);
            userDto.Token = _tokenProvider.GetToken(user);
            return Ok(userDto);
        }

        /// <summary>
        ///     Deletes the user
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.IS_OWNER, id);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _userService.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        ///     Gets all users
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.IS_ADMIN, null);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var users = await _userService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TeamResponseDto>>(users));
        }

        /// <summary>
        ///     Gets the user
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.IS_OWNER, id);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var user = await _userService.GetByIdAsync(id);
            return Ok(_mapper.Map<UserResponseDto>(user));
        }

        /// <summary>
        ///     Gets all card levels having been set for the user
        /// </summary>
        [HttpGet("{id}/card-levels")]
        public async Task<IActionResult> GetCardLevels(Guid id)
        {
            // TODO: secure
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.IS_OWNER, id);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var cardLevels = await _userService.GetCardLevelsAsync(id);
            return Ok(_mapper.Map<IEnumerable<CardLevelResponseDto>>(cardLevels));
        }

        /// <summary>
        ///     Gets all existing cards and includes the level for the user if it has been set
        /// </summary>
        [HttpGet("{id}/cards")]
        public async Task<IActionResult> GetCardsWithUserLevels(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.IS_OWNER, id);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var cardsWithUserLevels = await _cardService.GetForUserAsync(id);
            var userCardDtos = _mapper.Map<IEnumerable<UserCardResponseDto>>(cardsWithUserLevels.Select(kv => kv.Key)).ToList();
            foreach (var (cardDto, level) in cardsWithUserLevels)
            {
                userCardDtos.Single(d => cardDto.Id == d.CardId).Level = level;
            }

            return Ok(userCardDtos);
        }
    }
}