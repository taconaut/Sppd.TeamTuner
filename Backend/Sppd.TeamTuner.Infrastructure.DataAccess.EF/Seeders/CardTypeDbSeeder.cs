using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Common;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;

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

        public Task SeedAsync()
        {
            _cardTypeRepository.Add(new CardType
                                    {
                                        Id = Guid.Parse(TestingConstants.CardType.SPELL_ID),
                                        Name = "Spell"
                                    });
            _cardTypeRepository.Add(new CardType
                                    {
                                        Id = Guid.Parse(TestingConstants.CardType.CHARACTER_ID),
                                        Name = "Character"
                                    });
            _cardTypeRepository.Add(new CardType
                                    {
                                        Id = Guid.Parse(TestingConstants.CardType.SPAWN_ID),
                                        Name = "Spawn"
                                    });

            return Task.CompletedTask;
        }
    }
}