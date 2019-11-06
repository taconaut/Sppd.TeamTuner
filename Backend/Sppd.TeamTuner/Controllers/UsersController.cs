using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mapper">The mapper.</param>
        public UsersController(ITeamTunerUserService userService, ICardService cardService, ITokenProvider tokenProvider, IAuthorizationService authorizationService,
            IServiceProvider serviceProvider, IMapper mapper)
            : base(serviceProvider, authorizationService)
        {
            _userService = userService;
            _cardService = cardService;
            _tokenProvider = tokenProvider;
            _mapper = mapper;
        }

        /// <summary>
        ///     Creates a new user
        /// </summary>
        /// <param name="userCreateRequestDto">The user creation request</param>
        /// <remarks>
        ///     As stated by its name, the client has to send the MD5 hash of the password (length=36). This MD5 hash will be
        ///     stored as salted hash in the DB.
        /// </remarks>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] UserCreateRequestDto userCreateRequestDto)
        {
            var userToCreate = _mapper.Map<TeamTunerUser>(userCreateRequestDto);
            var createdUser = await _userService.CreateAsync(userToCreate, userCreateRequestDto.PasswordMd5);
            return Ok(_mapper.Map<UserResponseDto>(createdUser));
        }

        /// <summary>
        ///     Updates the user
        /// </summary>
        /// <param name="userRequestDto">The user update request</param>
        /// <remarks>
        ///     If the PropertiesToUpdate have been specified, only these will be updated; otherwise, all properties will be
        ///     updated.
        /// </remarks>
        [HttpPut]
        public async Task<ActionResult<UserResponseDto>> Update([FromBody] UserUpdateRequestDto userRequestDto)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_UPDATE_USER, userRequestDto.Id);
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
        /// <param name="authorizationRequestDto">The authorization request</param>
        /// <remarks>
        ///     The response contains a token which has to be included as bearer token in the HTTP authorization header for
        ///     subsequent calls to API methods requiring authentication.
        /// </remarks>
        [AllowAnonymous]
        [HttpPost("authorize")]
        public async Task<ActionResult<UserAuthorizationResponseDto>> Authorize([FromBody] AuthorizationRequestDto authorizationRequestDto)
        {
            var user = await _userService.AuthenticateAsync(authorizationRequestDto.Name, authorizationRequestDto.PasswordMd5);
            var userDto = _mapper.Map<UserAuthorizationResponseDto>(user);
            userDto.Token = _tokenProvider.GetToken(user);
            return Ok(userDto);
        }

        /// <summary>
        ///     Deletes the user
        /// </summary>
        /// <param name="id">The user identifier</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_DELETE_USER, id);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _userService.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        ///     Gets the user
        /// </summary>
        /// <param name="id">The user identifier</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetByUserId(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_READ_USER, id);
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
        /// <param name="id">The user identifier</param>
        [HttpGet("{id}/card-levels")]
        public async Task<ActionResult<IEnumerable<CardLevelResponseDto>>> GetCardLevels(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_READ_USER, id);
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
        /// <param name="id">The user identifier</param>
        [HttpGet("{id}/cards")]
        public async Task<ActionResult<IEnumerable<UserCardResponseDto>>> GetCardsWithUserLevels(Guid id)
        {
            var authorizationResult = await AuthorizeAsync(AuthorizationConstants.Policies.CAN_READ_USER, id);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var cardsWithUserLevels = await _cardService.GetForUserAsync(id);
            var userCardDtos = _mapper.Map<IEnumerable<UserCardResponseDto>>(cardsWithUserLevels.Select(kv => kv.Key)).ToList();

            // Add user attributes
            foreach (var (cardDto, cardLevel) in cardsWithUserLevels)
            {
                var userCardDto = userCardDtos.Single(d => cardDto.Id == d.CardId);
                SetLevelForUserCard(userCardDto, cardLevel, id);
            }

            return Ok(userCardDtos);
        }

        [AllowAnonymous]
        [HttpGet("email-verification/{code}/verify")]
        public async Task<ActionResult<bool>> VerifyEmail(string code)
        {
            return Ok(await _userService.VerifyEmailAsync(code));
        }

        [AllowAnonymous]
        [HttpGet("email-verification/{code}/resend")]
        public async Task<IActionResult> ResendVerificationMail(string code)
        {
            return Ok(await _userService.ResendEmailVerificationAsync(code));
        }

        private static void SetLevelForUserCard(UserCardResponseDto userCardDto, CardLevel cardLevel, Guid userId)
        {
            userCardDto.UserId = userId;
            userCardDto.Level = cardLevel?.Level;
            userCardDto.LevelLastModified = cardLevel?.ModifiedOnUtc;
        }
    }
}