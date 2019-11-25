using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Extensions.Logging;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;

namespace Sppd.TeamTuner.Infrastructure.Services
{
    internal class EmailVerificationService : IEmailVerificationService
    {
        private const string CODE_PLACEHOLDER = "{code}";
        private const char DELIMITER = ',';

        private readonly IEmailService _emailService;
        private readonly ITeamTunerUserRepository _userRepository;
        private readonly IRegistrationRequestRepository _registrationRequestRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmailVerificationService> _logger;
        private readonly Lazy<EmailConfig> _emailConfig;

        public EmailVerificationService(IEmailService emailService, ITeamTunerUserRepository userRepository, IConfigProvider<EmailConfig> emailConfig,
            IRegistrationRequestRepository registrationRequestRepository, IUnitOfWork unitOfWork, ILogger<EmailVerificationService> logger)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _registrationRequestRepository = registrationRequestRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailConfig = new Lazy<EmailConfig>(() => emailConfig.Config);
        }

        public async Task<bool> VerifyEmailAsync(string code)
        {
            var codeString = Base64Decode(HttpUtility.UrlDecode(code));
            var elements = codeString.Split(DELIMITER);
            var email = elements.ElementAt(0);
            var registrationCode = Guid.Parse(elements.ElementAt(1));

            try
            {
                var registrationRequest = await _registrationRequestRepository.GetByEmailAsync(email);
                if (registrationRequest != null
                    && registrationRequest.RegistrationCode == registrationCode
                    && registrationRequest.RegistrationDate.AddDays(3) > DateTime.UtcNow)
                {
                    // Delete the registration request
                    await _registrationRequestRepository.DeleteAsync(registrationRequest.Id);

                    // Update the email verified state of the user
                    var user = await _userRepository.GetByEmailAsync(email);
                    user.IsEmailVerified = true;

                    await _unitOfWork.CommitAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to validate email");
            }

            // If a user has this email address verified, the validation is also ok
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                return user.IsEmailVerified;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to find user with email '{email}'");
            }

            return false;
        }

        public async Task<bool> ResendEmailVerificationAsync(string code)
        {
            var codeString = Base64Decode(HttpUtility.UrlDecode(code));
            var elements = codeString.Split(DELIMITER);
            var email = elements.ElementAt(0);

            // Update the request date
            var registrationRequest = await _registrationRequestRepository.GetByEmailAsync(email);
            registrationRequest.RegistrationDate = DateTime.UtcNow;
            await _unitOfWork.CommitAsync();

            var user = await _userRepository.GetByEmailAsync(email);

            await SendEmailVerificationAsync(user, registrationRequest);

            return false;
        }

        public async Task SendEmailVerificationAsync(ITeamTunerUser user, TeamTunerUserRegistrationRequest registrationRequest)
        {
            // Prepare the email
            var subject = $"Confirm email for {user.Name} on Sppd.teamTuner";
            var emailConfirmationLink = GetConfirmationLink(registrationRequest);
            var body = $@"Hi {user.Name},

You have created an account for Sppd.teamTuner. If it wasn't you and you play South Park Phone Destroyer, well you should ;).

This link will be valid for 3 days.
Please click on the link to verify your Email address {emailConfirmationLink},

See you there, 

Your Sppd.TeamTuner team.";

            await _emailService.SendEmailAsync(subject, body, false, user.Email);
        }

        private string GetConfirmationLink(TeamTunerUserRegistrationRequest registrationRequest)
        {
            var emailVerificationUrl = _emailConfig.Value.EmailVerificationUrl;
            var codeString = string.Join(DELIMITER.ToString(), registrationRequest.User.Email, registrationRequest.RegistrationCode.ToString());
            var code = HttpUtility.UrlEncode(Base64Encode(codeString));
            return emailVerificationUrl.Replace(CODE_PLACEHOLDER, code);
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private static string Base64Decode(string base64Encoded)
        {
            var data = Convert.FromBase64String(base64Encoded);
            return Encoding.UTF8.GetString(data);
        }
    }
}