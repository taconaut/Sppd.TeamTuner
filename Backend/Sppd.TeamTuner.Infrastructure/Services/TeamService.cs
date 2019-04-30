using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;
using Sppd.TeamTuner.Core.Providers;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;

namespace Sppd.TeamTuner.Infrastructure.Services
{
    internal class TeamService : ServiceBase<Team>, ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IRepository<TeamTunerUser> _userRepository;
        private readonly ITeamMembershipRequestRepository _membershipRequestRepository;
        private readonly IEmailService _emailService;
        private readonly ITeamTunerUserProvider _userProvider;

        public TeamService(ITeamRepository teamRepository, IRepository<TeamTunerUser> userRepository, ITeamMembershipRequestRepository membershipRequestRepository,
            IEmailService emailService, IUnitOfWork unitOfWork, ITeamTunerUserProvider userProvider)
            : base(teamRepository, unitOfWork)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _membershipRequestRepository = membershipRequestRepository;
            _emailService = emailService;
            _userProvider = userProvider;
        }

        public override async Task<Team> CreateAsync(Team team)
        {
            await UpdateUser(team.Id);
            return await base.CreateAsync(team);
        }

        public async Task<IEnumerable<Team>> GetAllAsync(Guid federationId)
        {
            return await _teamRepository.GetAllAsync(federationId);
        }

        public async Task RequestMembershipAsync(Guid userId, Guid teamId, string comment)
        {
            var membershipRequest = new TeamMembershipRequest {UserId = userId, TeamId = teamId, Comment = comment};
            _membershipRequestRepository.Add(membershipRequest);
            await UnitOfWork.CommitAsync();

            // TODO: implement mailing
            //var joinRequestFull = await _joinRequestRepository.GetAsync(joinRequest.Id, new[] {nameof(TeamJoinRequest.User), nameof(TeamJoinRequest.Team)});
            // await _emailService.SendJoinRequestNotificationAsync(teamId, joinRequestFull);
        }

        public async Task AcceptMembershipAsync(Guid membershipRequestId)
        {
            var joinRequest = await _membershipRequestRepository.GetAsync(membershipRequestId, new[] {nameof(TeamMembershipRequest.User)});

            // Add user to team
            var user = joinRequest.User;
            user.TeamId = joinRequest.TeamId;
            user.TeamRole = CoreConstants.Authorization.Roles.MEMBER;

            // Job done, delete the request
            await _membershipRequestRepository.DeleteAsync(membershipRequestId);

            await UnitOfWork.CommitAsync();
        }

        public async Task RefuseMembershipAsync(Guid joinRequestId)
        {
            // The request has been refused, only delete the request
            await _membershipRequestRepository.DeleteAsync(joinRequestId);
            await UnitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<TeamMembershipRequest>> GetMembershipRequestsAsync(Guid teamId)
        {
            return await _membershipRequestRepository.GetForTeam(teamId);
        }

        public async Task<TeamMembershipRequest> GetMembershipRequestAsync(Guid membershipRequestId)
        {
            return await _membershipRequestRepository.GetAsync(membershipRequestId);
        }

        private async Task UpdateUser(Guid teamId)
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