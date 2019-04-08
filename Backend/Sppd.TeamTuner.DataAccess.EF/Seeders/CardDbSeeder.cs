using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Shared;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Seeders
{
    internal class CardDbSeeder : IDbSeeder
    {
        private readonly IRepository<Card> _cardRepository;

        public CardDbSeeder(IRepository<Card> cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public int Priority => SeederConstants.Priority.CARD_DATA;

        public Task SeedAsync()
        {
            // Neutral
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.TERRANCE_AND_PHILLIP_ID),
                                    Name = "Terrance and Phillip",
                                    FriendlyName = "Tap",
                                    ExternalId = 1680,
                                    ThemeId = Guid.Parse(TestingConstants.Theme.NEUTRAL_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.COMMON_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.FIGHTER_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.NATHAN_ID),
                                    Name = "Nathan",
                                    FriendlyName = "Nathan",
                                    ExternalId = 15,
                                    ThemeId = Guid.Parse(TestingConstants.Theme.NEUTRAL_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.RARE_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.RANGED_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.DOGPOO_ID),
                                    Name = "Dogpoo",
                                    FriendlyName = "Dogpoo",
                                    ExternalId = 1674,
                                    ThemeId = Guid.Parse(TestingConstants.Theme.NEUTRAL_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.EPIC_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.FIGHTER_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.SATAN_ID),
                                    Name = "Satan",
                                    FriendlyName = "Satan",
                                    ExternalId = 2080,
                                    ThemeId = Guid.Parse(TestingConstants.Theme.NEUTRAL_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.LEGENDARY_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.RANGED_ID)
                                });

            // Sci-Fy
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.ROBO_BEBE_ID),
                                    Name = "Robo Bebe",
                                    FriendlyName = "Robo",
                                    ExternalId = 1805,
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.COMMON_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.RANGED_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.ENFORCER_JIMMY_ID),
                                    Name = "Enforcer Jimmy",
                                    FriendlyName = "Enforcer",
                                    ExternalId = 209,
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.RARE_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.FIGHTER_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.AWESOMO_ID),
                                    Name = "A.W.E.S.O.M.-O 4000",
                                    FriendlyName = "awesomo",
                                    ExternalId = 38,
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.EPIC_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.TANK_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.MECHA_TIMMY_ID),
                                    Name = "Mecha Timmy",
                                    FriendlyName = "MT",
                                    ExternalId = 88,
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.LEGENDARY_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.RANGED_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.POISON_ID),
                                    Name = "Poison",
                                    FriendlyName = "Poison",
                                    ExternalId = 1657,
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.COMMON_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.SPELL_ID)
                                });

            // Fantasy
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.BLOOD_ELF_BEBE_ID),
                                    Name = "Blood Elf Bebe",
                                    FriendlyName = "BeB",
                                    ExternalId = 1806,
                                    ThemeId = Guid.Parse(TestingConstants.Theme.FANTASY_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.COMMON_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.RANGED_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.CANADIAN_KNIGHT_IKE_ID),
                                    Name = "Canadian Knight Ike",
                                    FriendlyName = "CKI",
                                    ExternalId = 144,
                                    ThemeId = Guid.Parse(TestingConstants.Theme.FANTASY_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.RARE_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.ASSASSIN_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.ROGUE_TOKEN_ID),
                                    Name = "Rogue Token",
                                    FriendlyName = "rogue",
                                    ExternalId = 54,
                                    ThemeId = Guid.Parse(TestingConstants.Theme.FANTASY_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.EPIC_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.FIGHTER_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.GRAND_WIZARD_CARTMAN_ID),
                                    Name = "Grand Wizard Cartman",
                                    FriendlyName = "GWC",
                                    ExternalId = 32,
                                    ThemeId = Guid.Parse(TestingConstants.Theme.FANTASY_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.LEGENDARY_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.TANK_ID)
                                });

            return Task.CompletedTask;
        }
    }
}