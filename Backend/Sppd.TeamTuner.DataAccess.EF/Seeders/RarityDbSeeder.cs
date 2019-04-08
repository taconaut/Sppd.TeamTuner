using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Shared;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Seeders
{
    internal class RarityDbSeeder : IDbSeeder
    {
        private readonly IRepository<Rarity> _rarityRepository;

        public RarityDbSeeder(IRepository<Rarity> rarityRepository)
        {
            _rarityRepository = rarityRepository;
        }

        public int Priority => SeederConstants.Priority.BASE_DATA;

        public Task SeedAsync()
        {
            _rarityRepository.Add(new Rarity
                                  {
                                      Id = new Guid(TestingConstants.Rarity.COMMON_ID),
                                      Name = "Common",
                                      FriendlyLevel = 4
                                  });
            _rarityRepository.Add(new Rarity
                                  {
                                      Id = new Guid(TestingConstants.Rarity.RARE_ID),
                                      Name = "Rare",
                                      FriendlyLevel = 3
                                  });
            _rarityRepository.Add(new Rarity
                                  {
                                      Id = new Guid(TestingConstants.Rarity.EPIC_ID),
                                      Name = "Epic",
                                      FriendlyLevel = 2
                                  });
            _rarityRepository.Add(new Rarity
                                  {
                                      Id = new Guid(TestingConstants.Rarity.LEGENDARY_ID),
                                      Name = "Legendary",
                                      FriendlyLevel = 1
                                  });

            return Task.CompletedTask;
        }
    }
}