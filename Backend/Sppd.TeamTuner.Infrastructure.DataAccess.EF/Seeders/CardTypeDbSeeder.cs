using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Common;

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
                                        Id = Guid.Parse(TestingConstants.CardType.ASSASSIN_ID),
                                        Name = "Assassin"
                                    });
            _cardTypeRepository.Add(new CardType
                                    {
                                        Id = Guid.Parse(TestingConstants.CardType.FIGHTER_ID),
                                        Name = "Fighter"
                                    });
            _cardTypeRepository.Add(new CardType
                                    {
                                        Id = Guid.Parse(TestingConstants.CardType.RANGED_ID),
                                        Name = "Ranged"
                                    });
            _cardTypeRepository.Add(new CardType
                                    {
                                        Id = Guid.Parse(TestingConstants.CardType.TANK_ID),
                                        Name = "Tank"
                                    });
            _cardTypeRepository.Add(new CardType
                                    {
                                        Id = Guid.Parse(TestingConstants.CardType.SPELL_ID),
                                        Name = "Spell"
                                    });

            return Task.CompletedTask;
        }
    }
}