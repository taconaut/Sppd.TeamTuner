using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Services;

namespace Sppd.TeamTuner.Infrastructure.Services
{
    internal class EmailService : IEmailService
    {
        private readonly Lazy<EmailConfig> _emailConfig;

        public EmailService(IConfigProvider<EmailConfig> emailConfigProvider)
        {
            _emailConfig = new Lazy<EmailConfig>(() => emailConfigProvider.Config);
        }

        public async Task SendEmailAsync(string subject, string body, bool isBodyHtml, string emailAddress)
        {
            await SendEmailAsync(subject, body, isBodyHtml, new[] {emailAddress});
        }

        public async Task SendEmailAsync(string subject, string body, bool isBodyHtml, IEnumerable<string> emailAddresses)
        {
            if (!_emailConfig.Value.IsSendMailEnabled)
            {
                return;
            }

            var emailAddressesList = emailAddresses.ToList();
            if (!emailAddressesList.Any())
            {
                throw new NotSupportedException("At least one email address has to be specified");
            }

            using (var message = new MailMessage())
            {
                foreach (var emailAddress in emailAddressesList)
                {
                    message.To.Add(new MailAddress(emailAddress));
                }

                message.From = new MailAddress(_emailConfig.Value.Account);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isBodyHtml;

                using (var client = new SmtpClient(_emailConfig.Value.SmtpServer))
                {
                    client.Port = _emailConfig.Value.Port;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_emailConfig.Value.Account, _emailConfig.Value.Password);
                    client.EnableSsl = true;
                    await client.SendMailAsync(message);
                }
            }
        }
    }
}