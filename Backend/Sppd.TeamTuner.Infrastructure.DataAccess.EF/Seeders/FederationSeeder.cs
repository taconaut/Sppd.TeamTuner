using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Common;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Seeders
{
    internal class FederationSeeder : IDbSeeder
    {
        private readonly IRepository<Federation> _federationRepository;

        public FederationSeeder(IRepository<Federation> federationRepository)
        {
            _federationRepository = federationRepository;
        }

        public int Priority => SeederConstants.Priority.TEST_DATA;

        public Task SeedAsync(SeedMode seedMode)
        {
            if (seedMode != SeedMode.Test)
            {
                return Task.CompletedTask;
            }

            _federationRepository.Add(new Federation
                                      {
                                          Id = new Guid(TestingConstants.Federation.HOLY_ID),
                                          Name = "Holy"
                                      });

            return Task.CompletedTask;
        }
    }
}