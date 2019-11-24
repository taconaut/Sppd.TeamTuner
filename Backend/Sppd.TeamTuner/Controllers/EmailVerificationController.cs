using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sppd.TeamTuner.Core.Services;

namespace Sppd.TeamTuner.Controllers
{
    /// <summary>
    ///     Handles email verifications.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Authorize]
    [ApiController]
    [Route("email-verification")]
    public class EmailVerificationController : ControllerBase
    {
        private readonly ITeamTunerUserService _userService;

        public EmailVerificationController(ITeamTunerUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        ///     Verifies the email with the given code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns><c>True</c> if the email could be verified; otherwise <c>false</c>.</returns>
        [AllowAnonymous]
        [HttpGet("{code}/verify")]
        public async Task<ActionResult<bool>> VerifyEmail(string code)
        {
            return Ok(await _userService.VerifyEmailAsync(code));
        }

        /// <summary>
        ///     Sends a previously sent verification email.
        /// </summary>
        /// <param name="code">The code.</param>
        [AllowAnonymous]
        [HttpGet("{code}/resend")]
        public async Task<IActionResult> ResendVerificationEmail(string code)
        {
            return Ok(await _userService.ResendEmailVerificationAsync(code));
        }
    }
}