using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Objects;
using Sppd.TeamTuner.Core.Exceptions;
using Sppd.TeamTuner.Core.Providers;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Core.Utils.Extensions;

using ArgumentException = Sppd.TeamTuner.Core.Exceptions.ArgumentException;

namespace Sppd.TeamTuner.Infrastructure.Services
{
    public class TeamTunerUserService : ServiceBase<TeamTunerUser>, ITeamTunerUserService
    {
        private readonly ITeamTunerUserRepository _teamTunerUserRepository;
        private readonly ICardLevelRepository _cardLevelRepository;
        private readonly IRegistrationRequestRepository _registrationRequestRepository;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly ITeamMembershipRequestRepository _teamMembershipRequestRepository;
        private readonly ICardRepository _cardRepository;
        private readonly ITeamTunerUserProvider _userProvider;
        private readonly Lazy<EmailConfig> _emailConfig;

        public TeamTunerUserService(ITeamTunerUserRepository teamTunerUserRepository, ICardLevelRepository cardLevelRepository,
            IRegistrationRequestRepository registrationRequestRepository, IEmailVerificationService emailVerificationService,
            ITeamMembershipRequestRepository teamMembershipRequestRepository, ICardRepository cardRepository, IUnitOfWork unitOfWork, ITeamTunerUserProvider userProvider,
            IConfigProvider<EmailConfig> emailConfigProvider)
            : base(teamTunerUserRepository, unitOfWork)
        {
            _teamTunerUserRepository = teamTunerUserRepository;
            _cardLevelRepository = cardLevelRepository;
            _registrationRequestRepository = registrationRequestRepository;
            _emailVerificationService = emailVerificationService;
            _teamMembershipRequestRepository = teamMembershipRequestRepository;
            _cardRepository = cardRepository;
            _userProvider = userProvider;
            _emailConfig = new Lazy<EmailConfig>(() => emailConfigProvider.Config);
        }

        public async Task<TeamTunerUser> AuthenticateAsync(string name, string passwordMd5)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(passwordMd5))
            {
                throw new SecurityException("Name or password incorrect");
            }

            var user = await _teamTunerUserRepository.GetByNameAsync(name);
            if (user == null)
            {
                throw new SecurityException("Name or password incorrect");
            }

            if (!VerifyPasswordHash(passwordMd5.ToUpper(), user.PasswordHash, user.PasswordSalt))
            {
                throw new SecurityException("Name or password incorrect");
            }

            if (!user.IsEmailVerified)
            {
                throw new SecurityException("Email has not been verified");
            }

            return user;
        }

        public override Task CreateAsync(TeamTunerUser entity, bool commitChanges = true)
        {
            throw new NotSupportedException($"Call method 'CreateAsync(TeamTunerUser user, string passwordMd5)' to create a {nameof(TeamTunerUser)}");
        }

        public async Task<TeamTunerUser> CreateAsync(TeamTunerUser user, string passwordMd5)
        {
            // Create the user
            await AddAsync(user, passwordMd5.ToUpper());

            // Create the registration request (used to validate the email)
            var registrationRequest = GetRegistrationRequest(user);
            _registrationRequestRepository.Add(registrationRequest);

            if (_emailConfig.Value.IsSendMailEnabled)
            {
                // Send the validation mail
                await _emailVerificationService.SendEmailVerificationAsync(user, registrationRequest);
            }
            else
            {
                user.IsEmailVerified = true;
            }

            // Persist to DB
            await UnitOfWork.CommitAsync();

            return user;
        }

        public async Task<bool> VerifyEmailAsync(string code)
        {
            return await _emailVerificationService.VerifyEmailAsync(code);
        }

        public async Task<bool> ResendEmailVerificationAsync(string code)
        {
            return await _emailVerificationService.ResendEmailVerificationAsync(code);
        }

        public async Task<TeamMembershipRequest> GetPendingTeamMembershipRequest(Guid userId)
        {
            return await _teamMembershipRequestRepository.GetForUser(userId,
                new[] {nameof(TeamMembershipRequest.User), $"{nameof(TeamMembershipRequest.Team)}.{nameof(Team.Users)}"});
        }

        public async Task<TeamTunerUser> LeaveTeamAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);

            if (user.TeamRole == CoreConstants.Authorization.Roles.LEADER)
            {
                throw new BusinessException("The team leader can't leave the team");
            }

            user.TeamId = null;
            user.Team = null;
            user.TeamRole = null;

            await UnitOfWork.CommitAsync();

            return user;
        }

        public async Task UpdateTeamRoleAsync(Guid userId, string role)
        {
            var user = await GetByIdAsync(userId);

            if (role == CoreConstants.Authorization.Roles.LEADER)
            {
                // Demote the current leader to CoLeader when setting a new leader (as there can only be a single team leader at any given time).
                var currentLeader = await GetByIdAsync(_userProvider.CurrentUser.Id);
                currentLeader.TeamRole = CoreConstants.Authorization.Roles.CO_LEADER;
            }

            user.TeamRole = role;

            await UnitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<UserCard>> GetCardsWithLevelsAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            var allCards = await _cardRepository.GetAllAsync();
            var userCardLevels = await _cardLevelRepository.GetAllForUserAsync(userId);

            return allCards.Select(card => new UserCard
                                           {
                                               Card = card,
                                               Level = userCardLevels.SingleOrDefault(cl => cl.CardId == card.Id),
                                               User = user
                                           });
        }

        public async Task<IEnumerable<TeamTunerUser>> GetByTeamIdAsync(Guid teamId)
        {
            return await _teamTunerUserRepository.GetByTeamIdAsync(teamId);
        }

        public async Task<IEnumerable<CardLevel>> GetCardLevelsAsync(Guid userId)
        {
            var user = await _teamTunerUserRepository.GetAsync(userId, new[] {nameof(TeamTunerUser.CardLevels)});
            return user.CardLevels;
        }

        public async Task<CardLevel> SetCardLevelAsync(CardLevel cardLevel)
        {
            CardLevel cardLevelToUpdate;
            try
            {
                cardLevelToUpdate = await _cardLevelRepository.GetAsync(cardLevel.UserId, cardLevel.CardId);
            }
            catch (EntityNotFoundException)
            {
                cardLevelToUpdate = new CardLevel();
                _cardLevelRepository.Add(cardLevelToUpdate);
            }

            cardLevelToUpdate.MapProperties(cardLevel, new[] {nameof(CardLevel.CardId), nameof(CardLevel.UserId), nameof(CardLevel.Level)});

            await UnitOfWork.CommitAsync();

            return cardLevelToUpdate;
        }

        public async Task AddAsync(TeamTunerUser user, string passwordMd5)
        {
            if (string.IsNullOrWhiteSpace(passwordMd5) || passwordMd5.Length != 32)
            {
                throw new SecurityException($"The specified {nameof(passwordMd5)} is not valid. It must have a length of 32 (md5 hash).");
            }

            var allUsers = (await _teamTunerUserRepository.GetAllAsync()).ToList();

            if (allUsers.Any(u => u.Name == user.Name))
            {
                throw new BusinessException($"{nameof(user.Name)}='{user.Name}' is already registered.");
            }

            if (allUsers.Any(u => u.Email == user.Email))
            {
                throw new BusinessException($"{nameof(user.Email)}='{user.Email}' is already registered.");
            }

            CreatePasswordHash(passwordMd5, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _teamTunerUserRepository.Add(user);
        }

        private static TeamTunerUserRegistrationRequest GetRegistrationRequest(TeamTunerUser user)
        {
            return new TeamTunerUserRegistrationRequest
                   {
                       UserId = user.Id,
                       User = user,
                       RegistrationCode = Guid.NewGuid(),
                       RegistrationDate = DateTime.UtcNow
                   };
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentException("Value is NULL", nameof(password));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            }

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
            {
                throw new ArgumentException("Value is NULL", nameof(password));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            }

            if (storedHash.Length != CoreConstants.ArrayLength.TeamTunerUser.PASSWORD_HASH)
            {
                throw new ArgumentException($"Invalid length of password hash ({CoreConstants.ArrayLength.TeamTunerUser.PASSWORD_HASH} bytes expected).", nameof(storedHash));
            }

            if (storedSalt.Length != CoreConstants.ArrayLength.TeamTunerUser.PASSWORD_SALT)
            {
                throw new ArgumentException($"Invalid length of password salt ({CoreConstants.ArrayLength.TeamTunerUser.PASSWORD_SALT} bytes expected).", nameof(storedHash));
            }

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}