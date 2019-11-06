using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sppd.TeamTuner.Core.Services
{
    /// <summary>
    ///     A service to send mails using the smtp server set in the configuration.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        ///     Sends an email to <see cref="emailAddress" />.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> the mail will be rendered as HTML.</param>
        /// <param name="emailAddress">The email address.</param>
        Task SendEmailAsync(string subject, string body, bool isBodyHtml, string emailAddress);

        /// <summary>
        ///     Sends an email to all <see cref="emailAddresses" />.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> the mail will be rendered as HTML.</param>
        /// <param name="emailAddresses">The email addresses.</param>
        /// <returns></returns>
        Task SendEmailAsync(string subject, string body, bool isBodyHtml, IEnumerable<string> emailAddresses);
    }
}