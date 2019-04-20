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
        private readonly ITeamJoinRequestRepository _joinRequestRepository;
        private readonly IEmailService _emailService;
        private readonly ITeamTunerUserProvider _userProvider;

        public TeamService(ITeamRepository teamRepository, IRepository<TeamTunerUser> userRepository, ITeamJoinRequestRepository joinRequestRepository,
            IEmailService emailService, IUnitOfWork unitOfWork, ITeamTunerUserProvider userProvider)
            : base(teamRepository, unitOfWork)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _joinRequestRepository = joinRequestRepository;
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

        public async Task RequestJoinAsync(Guid userId, Guid teamId, string comment)
        {
            var joinRequest = new TeamJoinRequest {UserId = userId, TeamId = teamId, Comment = comment};
            _joinRequestRepository.Add(joinRequest);
            await UnitOfWork.CommitAsync();

            // TODO: implement mailing
            //var joinRequestFull = await _joinRequestRepository.GetAsync(joinRequest.Id, new[] {nameof(TeamJoinRequest.User), nameof(TeamJoinRequest.Team)});
            // await _emailService.SendJoinRequestNotificationAsync(teamId, joinRequestFull);
        }

        public async Task AcceptJoinAsync(Guid joinRequestId)
        {
            var joinRequest = await _joinRequestRepository.GetAsync(joinRequestId, new[] {nameof(TeamJoinRequest.User)});

            // Add user to team
            var user = joinRequest.User;
            user.TeamId = joinRequest.TeamId;
            user.TeamRole = CoreConstants.Auth.Roles.MEMBER;

            // Job done, delete the request
            await _joinRequestRepository.DeleteAsync(joinRequestId);

            await UnitOfWork.CommitAsync();
        }

        public async Task RefuseJoinAsync(Guid joinRequestId)
        {
            // The request has been refused, only delete the request
            await _joinRequestRepository.DeleteAsync(joinRequestId);
            await UnitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<TeamJoinRequest>> GetJoinRequestsAsync(Guid teamId)
        {
            return await _joinRequestRepository.GetForTeam(teamId);
        }

        public async Task<TeamJoinRequest> GetJoinRequestAsync(Guid joinRequestId)
        {
            return await _joinRequestRepository.GetAsync(joinRequestId);
        }

        private async Task UpdateUser(Guid teamId)
        {
            var user = await _userRepository.GetAsync(_userProvider.CurrentUser.Id);

            if (user.TeamId.HasValue)
            {
                throw new BusinessException("The user can't create a new team while he is part of another one.");
            }

            user.TeamId = teamId;
            user.TeamRole = CoreConstants.Auth.Roles.LEADER;
            _userRepository.Update(user);

            _userProvider.CurrentUser = user;
        }
    }
}