using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Common;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Seeders
{
    internal class CharacterTypeDbSeeder : IDbSeeder
    {
        private readonly IRepository<CharacterType> _characterTypeRepository;

        public CharacterTypeDbSeeder(IRepository<CharacterType> characterTypeRepository)
        {
            _characterTypeRepository = characterTypeRepository;
        }

        public int Priority => SeederConstants.Priority.BASE_DATA;

        public Task SeedAsync()
        {
            _characterTypeRepository.Add(new CharacterType
                                         {
                                             Id = Guid.Parse(TestingConstants.CharacterType.ASSASSIN_ID),
                                             Name = "Assassin"
                                         });
            _characterTypeRepository.Add(new CharacterType
                                         {
                                             Id = Guid.Parse(TestingConstants.CharacterType.MELEE_ID),
                                             Name = "Melee"
                                         });
            _characterTypeRepository.Add(new CharacterType
                                         {
                                             Id = Guid.Parse(TestingConstants.CharacterType.RANGED_ID),
                                             Name = "Ranged"
                                         });
            _characterTypeRepository.Add(new CharacterType
                                         {
                                             Id = Guid.Parse(TestingConstants.CharacterType.TANK_ID),
                                             Name = "Tank"
                                         });
            _characterTypeRepository.Add(new CharacterType
                                         {
                                             Id = Guid.Parse(TestingConstants.CharacterType.TOTEM_ID),
                                             Name = "Totem"
                                         });

            return Task.CompletedTask;
        }
    }
}