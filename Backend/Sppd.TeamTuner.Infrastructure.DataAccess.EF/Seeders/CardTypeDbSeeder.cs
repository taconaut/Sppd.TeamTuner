using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Common;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Seeders
{
    internal class CardTypeDbSeeder : IDbSeeder
    {
        private readonly IRepository<CardType> _cardTypeRepository;

        public CardTypeDbSeeder(IRepository<CardType> cardTypeRepository)
        {
            _cardTypeRepository = cardTypeRepository;
        }

        public int Priority => SeederConstants.Priority.BASE_DATA;

        public Task SeedAsync(SeedMode seedMode)
        {
            if (seedMode == SeedMode.None)
            {
                return Task.CompletedTask;
            }

            _cardTypeRepository.Add(new CardType
                                    {
                                        Id = Guid.Parse(CoreDataConstants.CardType.SPELL_ID),
                                        Name = "Spell"
                                    });
            _cardTypeRepository.Add(new CardType
                                    {
                                        Id = Guid.Parse(CoreDataConstants.CardType.CHARACTER_ID),
                                        Name = "Character"
                                    });
            _cardTypeRepository.Add(new CardType
                                    {
                                        Id = Guid.Parse(CoreDataConstants.CardType.SPAWN_ID),
                                        Name = "Spawn"
                                    });
            _cardTypeRepository.Add(new CardType
                                    {
                                        Id = Guid.Parse(CoreDataConstants.CardType.TRAP_ID),
                                        Name = "Trap"
                                    });

            return Task.CompletedTask;
        }
    }
}