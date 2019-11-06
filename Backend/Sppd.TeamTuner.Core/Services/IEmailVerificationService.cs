using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Interfaces;

namespace Sppd.TeamTuner.Core.Services
{
    /// <summary>
    ///     Offers services used for email verification.
    /// </summary>
    public interface IEmailVerificationService
    {
        /// <summary>
        ///     Sends a mail containing a link to verify the email address by <see cref="VerifyEmailAsync" /> to
        ///     <see cref="user" />.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="registrationRequest">The registration request.</param>
        Task SendEmailVerificationAsync(ITeamTunerUser user, TeamTunerUserRegistrationRequest registrationRequest);

        /// <summary>
        ///     Verifies the email.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns><c>True</c> if the verification succeeded; otherwise <c>false</c>.</returns>
        Task<bool> VerifyEmailAsync(string code);

        /// <summary>
        ///     Sends the same mail, previously sent by <see cref="SendEmailVerificationAsync" />.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns><c>True</c> if the mail could be sent; otherwise <c>false</c>.</returns>
        Task<bool> ResendEmailVerificationAsync(string code);
    }
}