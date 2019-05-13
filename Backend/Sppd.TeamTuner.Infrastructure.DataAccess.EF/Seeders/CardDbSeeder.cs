using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Common;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;

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
                                    FriendlyNames = new[] {"Tap"},
                                    ExternalId = "5caba68ac83b14195097bf40",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.NEUTRAL_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.COMMON_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.FIGHTER_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.NATHAN_ID),
                                    Name = "Nathan",
                                    FriendlyNames = new[] {"Nathan"},
                                    ExternalId = "5caf4b17422efa19c93ef40a",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.NEUTRAL_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.RARE_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.RANGED_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.DOGPOO_ID),
                                    Name = "Dogpoo",
                                    FriendlyNames = new[] {"Dogpoo"},
                                    ExternalId = "5caba68ac83b14195097bf3f",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.NEUTRAL_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.EPIC_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.FIGHTER_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.SATAN_ID),
                                    Name = "Satan",
                                    FriendlyNames = new[] {"Satan"},
                                    ExternalId = "5caba68fc83b14195097bf59",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.NEUTRAL_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.LEGENDARY_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.RANGED_ID)
                                });

            // Sci-Fy
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.ROBO_BEBE_ID),
                                    Name = "Robo Bebe",
                                    FriendlyNames = new[] {"Robo"},
                                    ExternalId = "5caba68cc83b14195097bf49",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.COMMON_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.RANGED_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.ENFORCER_JIMMY_ID),
                                    Name = "Enforcer Jimmy",
                                    FriendlyNames = new[] {"Enforcer"},
                                    ExternalId = "5caba684c83b14195097bf24",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.RARE_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.FIGHTER_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.AWESOMO_ID),
                                    Name = "A.W.E.S.O.M.-O 4000",
                                    FriendlyNames = new[] {"awesomo"},
                                    ExternalId = "5caba676c83b14195097bef8",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.EPIC_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.TANK_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.MECHA_TIMMY_ID),
                                    Name = "Mecha Timmy",
                                    FriendlyNames = new[] {"MT"},
                                    ExternalId = "5caba67ec83b14195097bf0c",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.LEGENDARY_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.RANGED_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.POISON_ID),
                                    Name = "Poison",
                                    FriendlyNames = new[] {"Poison"},
                                    ExternalId = "5caba688c83b14195097bf39",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.COMMON_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.SPELL_ID)
                                });

            // Fantasy
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.BLOOD_ELF_BEBE_ID),
                                    Name = "Blood Elf Bebe",
                                    FriendlyNames = new[] {"BeB"},
                                    ExternalId = "5caba68cc83b14195097bf4a",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.FANTASY_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.COMMON_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.RANGED_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.CANADIAN_KNIGHT_IKE_ID),
                                    Name = "Canadian Knight Ike",
                                    FriendlyNames = new[] {"CKI"},
                                    ExternalId = "5caba681c83b14195097bf18",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.FANTASY_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.RARE_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.ASSASSIN_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.ROGUE_TOKEN_ID),
                                    Name = "Rogue Token",
                                    FriendlyNames = new[] {"rogue"},
                                    ExternalId = "5caba67cc83b14195097bf04",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.FANTASY_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.EPIC_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.FIGHTER_ID)
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.GRAND_WIZARD_CARTMAN_ID),
                                    Name = "Grand Wizard Cartman",
                                    FriendlyNames = new[] {"GWC"},
                                    ExternalId = "5caba675c83b14195097bef5",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.FANTASY_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.LEGENDARY_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.TANK_ID)
                                });

            return Task.CompletedTask;
        }
    }
}