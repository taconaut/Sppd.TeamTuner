using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Services;

namespace Sppd.TeamTuner.Infrastructure.Services
{
    internal class EmailService : IEmailService
    {
        private readonly ITeamTunerUserService _userService;
        private readonly ILogger<EmailService> _logger;
        private readonly Lazy<EmailConfig> _emailConfig;

        public EmailService(ITeamTunerUserService userService, IConfigProvider<EmailConfig> emailConfigProvider, ILogger<EmailService> logger)
        {
            _userService = userService;
            _logger = logger;
            _emailConfig = new Lazy<EmailConfig>(() => emailConfigProvider.Config);
        }

        public async Task SendMembershipRequestNotificationAsync(Guid teamId, TeamMembershipRequest membershipRequest)
        {
            var teamUsers = await _userService.GetByTeamIdAsync(teamId);
            var usersToNotify = teamUsers.Where(u => u.TeamRole == CoreConstants.Authorization.Roles.LEADER || u.TeamRole == CoreConstants.Authorization.Roles.CO_LEADER);

            var team = membershipRequest.Team;

            var subject = $"A user requested to join {team.Name}";
            var body = "You can grand him access from here: 'TODO: add URL'";

            await SendEmail(subject, body, false, usersToNotify.Select(u => u.Email));
        }

        private async Task SendEmail(string subject, string body, bool isBodyHtml, IEnumerable<string> emailAddresses)
        {
            using (var message = new MailMessage())
            {
                var emailAddressesList = emailAddresses.ToList();
                foreach (var emailAddress in emailAddressesList)
                {
                    message.To.Add(new MailAddress(emailAddress));
                }

                message.Sender = new MailAddress(_emailConfig.Value.Account);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isBodyHtml;

                using (var client = new SmtpClient(_emailConfig.Value.SmtpServer))
                {
                    client.Port = _emailConfig.Value.Port;
                    client.Credentials = new NetworkCredential(_emailConfig.Value.Account, _emailConfig.Value.Password);
                    client.EnableSsl = true;
                    try
                    {
                        await client.SendMailAsync(message);
                        _logger.LogDebug($"Email with subject '{subject}' has been sent to {string.Join(", ", emailAddressesList)}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Failed to send email with subject '{subject}' to {string.Join(", ", emailAddressesList)}");
                    }
                }
            }
        }
    }
}