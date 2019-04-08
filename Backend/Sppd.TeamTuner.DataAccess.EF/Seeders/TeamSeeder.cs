using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Shared;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Seeders
{
    internal class TeamSeeder : IDbSeeder
    {
        private readonly IRepository<Team> _teamRepository;

        public TeamSeeder(IRepository<Team> teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public int Priority => SeederConstants.Priority.TEST_DATA;

        public Task SeedAsync()
        {
            _teamRepository.Add(new Team
                                {
                                    Id = new Guid(TestingConstants.Team.HOLY_COW),
                                    FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                    Name = "Holy Cow"
                                });
            _teamRepository.Add(new Team
                                {
                                    Id = new Guid(TestingConstants.Team.HOLY_SHIT),
                                    FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                    Name = "Holy Shit"
                                });
            _teamRepository.Add(new Team
                                {
                                    Id = new Guid(TestingConstants.Team.UNHOLY),
                                    FederationId = new Guid(TestingConstants.Federation.HOLY_ID),
                                    Name = "Unholy"
                                });

            return Task.CompletedTask;
        }
    }
}