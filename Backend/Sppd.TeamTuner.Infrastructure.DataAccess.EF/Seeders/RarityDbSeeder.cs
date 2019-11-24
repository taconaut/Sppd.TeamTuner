using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Common;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config;

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

        public Task SeedAsync(SeedMode seedMode)
        {
            if (seedMode == SeedMode.None)
            {
                return Task.CompletedTask;
            }

            _rarityRepository.Add(new Rarity
                                  {
                                      Id = new Guid(CoreDataConstants.Rarity.COMMON_ID),
                                      Name = "Common",
                                      FriendlyLevel = 4
                                  });
            _rarityRepository.Add(new Rarity
                                  {
                                      Id = new Guid(CoreDataConstants.Rarity.RARE_ID),
                                      Name = "Rare",
                                      FriendlyLevel = 3
                                  });
            _rarityRepository.Add(new Rarity
                                  {
                                      Id = new Guid(CoreDataConstants.Rarity.EPIC_ID),
                                      Name = "Epic",
                                      FriendlyLevel = 2
                                  });
            _rarityRepository.Add(new Rarity
                                  {
                                      Id = new Guid(CoreDataConstants.Rarity.LEGENDARY_ID),
                                      Name = "Legendary",
                                      FriendlyLevel = 1
                                  });

            return Task.CompletedTask;
        }
    }
}