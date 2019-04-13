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
        private readonly IRepository<TeamTunerUser> _teamTunerUserRepository;
        private readonly ITeamTunerUserProvider _userProvider;

        public TeamService(ITeamRepository teamRepository, IRepository<TeamTunerUser> teamTunerUserRepository,
            IUnitOfWork unitOfWork, ITeamTunerUserProvider userProvider)
            : base(teamRepository, unitOfWork)
        {
            _teamRepository = teamRepository;
            _teamTunerUserRepository = teamTunerUserRepository;
            _userProvider = userProvider;
        }

        public override async Task<Team> CreateAsync(Team entity)
        {
            await UpdateUser(entity);
            return await base.CreateAsync(entity);
        }

        public async Task<IEnumerable<Team>> GetAllAsync(Guid federationId)
        {
            return await _teamRepository.GetAllAsync(federationId);
        }

        private async Task UpdateUser(Team entity)
        {
            var user = await _teamTunerUserRepository.GetAsync(_userProvider.CurrentUser.Id);

            if (user.TeamId.HasValue)
            {
                throw new BusinessException("The user can't create a new team while he is part of another one.");
            }

            user.TeamId = entity.Id;
            user.TeamRole = CoreConstants.Auth.Roles.LEADER;
            _teamTunerUserRepository.Update(user);

            _userProvider.CurrentUser = user;
        }
    }
}