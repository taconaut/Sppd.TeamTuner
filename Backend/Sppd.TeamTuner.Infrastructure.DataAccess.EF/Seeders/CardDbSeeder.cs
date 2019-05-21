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
                                    ExternalId = "5caba68ac83b14195097bf40",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.NEUTRAL_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.COMMON_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.CHARACTER_ID),
                                    CharacterTypeId = Guid.Parse(TestingConstants.CharacterType.MELEE_ID),
                                    ManaCost = 4
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.NATHAN_ID),
                                    Name = "Nathan",
                                    ExternalId = "5caf4b17422efa19c93ef40a",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.NEUTRAL_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.RARE_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.CHARACTER_ID),
                                    CharacterTypeId = Guid.Parse(TestingConstants.CharacterType.RANGED_ID),
                                    ManaCost = 4
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.DOGPOO_ID),
                                    Name = "Dogpoo",
                                    ExternalId = "5caba68ac83b14195097bf3f",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.NEUTRAL_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.EPIC_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.CHARACTER_ID),
                                    CharacterTypeId = Guid.Parse(TestingConstants.CharacterType.MELEE_ID),
                                    ManaCost = 3
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.SATAN_ID),
                                    Name = "Satan",
                                    ExternalId = "5caba68fc83b14195097bf59",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.NEUTRAL_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.LEGENDARY_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.CHARACTER_ID),
                                    CharacterTypeId = Guid.Parse(TestingConstants.CharacterType.RANGED_ID),
                                    ManaCost = 6
                                });

            // Sci-Fy
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.ROBO_BEBE_ID),
                                    Name = "Robo Bebe",
                                    ExternalId = "5caba68cc83b14195097bf49",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.COMMON_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.CHARACTER_ID),
                                    CharacterTypeId = Guid.Parse(TestingConstants.CharacterType.RANGED_ID),
                                    ManaCost = 3
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.ENFORCER_JIMMY_ID),
                                    Name = "Enforcer Jimmy",
                                    ExternalId = "5caba684c83b14195097bf24",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.RARE_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.CHARACTER_ID),
                                    CharacterTypeId = Guid.Parse(TestingConstants.CharacterType.MELEE_ID),
                                    ManaCost = 2
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.AWESOMO_ID),
                                    Name = "A.W.E.S.O.M.-O 4000",
                                    ExternalId = "5caba676c83b14195097bef8",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.EPIC_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.CHARACTER_ID),
                                    CharacterTypeId = Guid.Parse(TestingConstants.CharacterType.TANK_ID),
                                    ManaCost = 5
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.MECHA_TIMMY_ID),
                                    Name = "Mecha Timmy",
                                    ExternalId = "5caba67ec83b14195097bf0c",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.LEGENDARY_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.CHARACTER_ID),
                                    CharacterTypeId = Guid.Parse(TestingConstants.CharacterType.RANGED_ID),
                                    ManaCost = 4
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.POISON_ID),
                                    Name = "Poison",
                                    ExternalId = "5caba688c83b14195097bf39",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.SCIFI_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.COMMON_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.SPELL_ID),
                                    ManaCost = 3
                                });

            // Fantasy
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.BLOOD_ELF_BEBE_ID),
                                    Name = "Blood Elf Bebe",
                                    ExternalId = "5caba68cc83b14195097bf4a",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.FANTASY_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.COMMON_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.CHARACTER_ID),
                                    CharacterTypeId = Guid.Parse(TestingConstants.CharacterType.RANGED_ID),
                                    ManaCost = 3
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.CANADIAN_KNIGHT_IKE_ID),
                                    Name = "Canadian Knight Ike",
                                    ExternalId = "5caba681c83b14195097bf18",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.FANTASY_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.RARE_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.CHARACTER_ID),
                                    CharacterTypeId = Guid.Parse(TestingConstants.CharacterType.ASSASSIN_ID),
                                    ManaCost = 3
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.ROGUE_TOKEN_ID),
                                    Name = "Rogue Token",
                                    ExternalId = "5caba67cc83b14195097bf04",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.FANTASY_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.EPIC_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.CHARACTER_ID),
                                    CharacterTypeId = Guid.Parse(TestingConstants.CharacterType.MELEE_ID),
                                    ManaCost = 4
                                });
            _cardRepository.Add(new Card
                                {
                                    Id = Guid.Parse(TestingConstants.Card.GRAND_WIZARD_CARTMAN_ID),
                                    Name = "Grand Wizard Cartman",
                                    ExternalId = "5caba675c83b14195097bef5",
                                    ThemeId = Guid.Parse(TestingConstants.Theme.FANTASY_ID),
                                    RarityId = Guid.Parse(TestingConstants.Rarity.LEGENDARY_ID),
                                    TypeId = Guid.Parse(TestingConstants.CardType.CHARACTER_ID),
                                    CharacterTypeId = Guid.Parse(TestingConstants.CharacterType.TANK_ID),
                                    ManaCost = 6
                                });

            return Task.CompletedTask;
        }
    }
}