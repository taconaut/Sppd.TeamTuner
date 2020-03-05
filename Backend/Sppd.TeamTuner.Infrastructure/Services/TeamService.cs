using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Objects;
using Sppd.TeamTuner.Core.Exceptions;
using Sppd.TeamTuner.Core.Providers;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;

namespace Sppd.TeamTuner.Infrastructure.Services
{
    internal class TeamService : ServiceBase<Team>, ITeamService
    {
        private readonly IEmailService _emailService;
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamTunerUserRepository _userRepository;
        private readonly ITeamMembershipRequestRepository _membershipRequestRepository;
        private readonly ICardRepository _cardRepository;
        private readonly ICardLevelRepository _cardLevelRepository;
        private readonly ITeamTunerUserProvider _userProvider;

        public TeamService(IEmailService emailService, ITeamRepository teamRepository, ITeamTunerUserRepository userRepository,
            ITeamMembershipRequestRepository membershipRequestRepository, ICardRepository cardRepository, ICardLevelRepository cardLevelRepository, IUnitOfWork unitOfWork,
            ITeamTunerUserProvider userProvider)
            : base(teamRepository, unitOfWork)
        {
            _emailService = emailService;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _membershipRequestRepository = membershipRequestRepository;
            _cardRepository = cardRepository;
            _cardLevelRepository = cardLevelRepository;
            _userProvider = userProvider;
        }

        public override async Task CreateAsync(Team team, bool commitChanges = true)
        {
            await MakeCreatingUserLeader(team.Id);
            await base.CreateAsync(team, commitChanges);
        }

        public async Task<IEnumerable<Team>> GetAllAsync(Guid federationId)
        {
            return await _teamRepository.GetAllAsync(federationId);
        }

        public async Task<IEnumerable<Team>> SearchByNameAsync(string teamName)
        {
            return await _teamRepository.SearchByNameAsync(teamName);
        }

        public async Task RequestMembershipAsync(Guid userId, Guid teamId, string comment)
        {
            var membershipRequest = new TeamMembershipRequest {UserId = userId, TeamId = teamId, Comment = comment};
            _membershipRequestRepository.Add(membershipRequest);
            await UnitOfWork.CommitAsync();

            await SendMembershipRequestEmailAsync(userId);
        }

        public async Task AcceptMembershipAsync(Guid membershipRequestId)
        {
            var membershipRequest = await _membershipRequestRepository.GetAsync(membershipRequestId, new[] {nameof(TeamMembershipRequest.User)});

            // Add user to team
            var user = membershipRequest.User;
            user.TeamId = membershipRequest.TeamId;
            user.TeamRole = CoreConstants.Authorization.Roles.MEMBER;

            // Job done, delete the request
            await _membershipRequestRepository.DeleteAsync(membershipRequestId);

            await UnitOfWork.CommitAsync();
        }

        public async Task RejectMembershipAsync(Guid joinRequestId)
        {
            // The request has been refused, only delete the request
            await _membershipRequestRepository.DeleteAsync(joinRequestId);
            await UnitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<TeamMembershipRequest>> GetMembershipRequestsAsync(Guid teamId)
        {
            return await _membershipRequestRepository.GetForTeam(teamId, new[] {nameof(TeamMembershipRequest.User)});
        }

        public async Task<TeamMembershipRequest> GetMembershipRequestAsync(Guid membershipRequestId)
        {
            return await _membershipRequestRepository.GetAsync(membershipRequestId);
        }

        public async Task AbortMembershipRequestAsync(Guid membershipRequestId)
        {
            await _membershipRequestRepository.DeleteAsync(membershipRequestId);
            await UnitOfWork.CommitAsync();
        }

        public async Task<TeamCards> GetCardsAsync(Guid teamId)
        {
            var allCards = await _cardRepository.GetAllAsync();
            var team = await Repository.GetAsync(teamId, new[] {nameof(Team.Users)});
            var cardLevels = team.Users
                                 .Select(async user => await _cardLevelRepository.GetAllForUserAsync(user.Id))
                                 .SelectMany(task => task.Result);

            return new TeamCards
                   {
                       Team = team,
                       Cards = allCards.Select(card => new TeamCard
                                               {
                                                   Card = card,
                                                   Levels = cardLevels.Where(cl => cl.CardId == card.Id)
                                                                      .GroupBy(cl => cl.Level, cl => cl.User)
                                                                      .ToDictionary(gr => gr.Key, gr => gr.Select(cl => cl))
                                               })
                   };
        }

        private async Task SendMembershipRequestEmailAsync(Guid userId)
        {
            var membershipRequest = await _membershipRequestRepository.GetForUser(userId,
                new[] {nameof(TeamMembershipRequest.User), $"{nameof(TeamMembershipRequest.Team)}.{nameof(Team.Users)}"});

            var subject = $"{membershipRequest.User.Name} would like to join {membershipRequest.Team.Name}";
            var body = $@"Hi {membershipRequest.Team.Name} leaders and co-leaders,

{membershipRequest.User.Name} would like to join your team.
You can let him in here: TODO: add link

Your Sppd.TeamTuner team.";
            var mailTo = membershipRequest.Team.CoLeaders.Select(user => user.Email)
                                          .Append(membershipRequest.Team.Leader.Email);

            await _emailService.SendEmailAsync(subject, body, false, mailTo);
        }

        private async Task MakeCreatingUserLeader(Guid teamId)
        {
            var user = await _userRepository.GetAsync(_userProvider.CurrentUser.Id);

            if (user.TeamId.HasValue)
            {
                throw new BusinessException("The user can't create a new team while he is part of another one.");
            }

            user.TeamId = teamId;
            user.TeamRole = CoreConstants.Authorization.Roles.LEADER;
            _userRepository.Update(user);

            _userProvider.CurrentUser = user;
        }
    }
}