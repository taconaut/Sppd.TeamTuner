using System;
using System.Text.RegularExpressions;

using AutoMapper;

using Sppd.TeamTuner.Common;
using Sppd.TeamTuner.Infrastructure.Feinwaru.Domain.Objects;

namespace Sppd.TeamTuner.Infrastructure.Feinwaru.MappingProfiles
{
    internal class CardMappingProfile : Profile
    {
        public CardMappingProfile()
        {
            CreateMap<Card, Core.Domain.Entities.Card>()
                // Map source to destination values
                .ForMember(dst => dst.Name, exp => exp.MapFrom(src => src.Name))
                // Replace any placeholder (e.g. {power_duration}) and set the value 'x', as we don't have the real value
                .ForMember(dst => dst.Description, exp => exp.MapFrom(src => Regex.Replace(src.Description, @" ?{.*?}", " x")))
                .ForMember(dst => dst.ManaCost, exp => exp.MapFrom(src => src.ManaCost))
                .ForMember(dst => dst.ExternalId, exp => exp.MapFrom(src => src.Id))
                .ForMember(dst => dst.ThemeId, exp => exp.MapFrom(src => GetThemeId(src.Theme)))
                .ForMember(dst => dst.RarityId, exp => exp.MapFrom(src => GetRarityId(src.Rarity)))
                .ForMember(dst => dst.TypeId, exp => exp.MapFrom(src => GetCardTypeId(src.Type, src.CharacterType)))

                // Use constant value
                .ForMember(dst => dst.IsDeleted, exp => exp.MapFrom(src => false))

                // Ignore navigation properties
                .ForMember(dst => dst.Theme, exp => exp.Ignore())
                .ForMember(dst => dst.Rarity, exp => exp.Ignore())
                .ForMember(dst => dst.Type, exp => exp.Ignore())

                // Ignore all properties from BaseEntity
                .ForMember(dst => dst.Id, exp => exp.Ignore())
                .ForMember(dst => dst.CreatedOnUtc, exp => exp.Ignore())
                .ForMember(dst => dst.CreatedById, exp => exp.Ignore())
                .ForMember(dst => dst.ModifiedOnUtc, exp => exp.Ignore())
                .ForMember(dst => dst.ModifiedById, exp => exp.Ignore())
                .ForMember(dst => dst.DeletedOnUtc, exp => exp.Ignore())
                .ForMember(dst => dst.DeletedById, exp => exp.Ignore())
                .ForMember(dst => dst.Version, exp => exp.Ignore());
        }

        private static Guid GetThemeId(string themeName)
        {
            switch (themeName)
            {
                case "sci-fi":
                    return Guid.Parse(TestingConstants.Theme.SCIFI_ID);

                case "adventure":
                    return Guid.Parse(TestingConstants.Theme.ADVENTURE_ID);

                case "fantasy":
                    return Guid.Parse(TestingConstants.Theme.FANTASY_ID);

                case "mystical":
                    return Guid.Parse(TestingConstants.Theme.MYSTICAL_ID);

                case "general":
                    return Guid.Parse(TestingConstants.Theme.NEUTRAL_ID);

                default:
                    throw new NotSupportedException($"Theme '{themeName}' is not supported");
            }
        }

        private static Guid GetRarityId(int rarity)
        {
            switch (rarity)
            {
                case 0:
                    return Guid.Parse(TestingConstants.Rarity.COMMON_ID);

                case 1:
                    return Guid.Parse(TestingConstants.Rarity.RARE_ID);

                case 2:
                    return Guid.Parse(TestingConstants.Rarity.EPIC_ID);

                case 3:
                    return Guid.Parse(TestingConstants.Rarity.LEGENDARY_ID);

                default:
                    throw new NotSupportedException($"Rarity '{rarity}' is not supported");
            }
        }

        private static Guid GetCardTypeId(string type, string characterType)
        {
            switch (type)
            {
                case "spell":
                    return Guid.Parse(TestingConstants.CardType.SPELL_ID);

                case "character":
                case "spawn":
                    switch (characterType)
                    {
                        case "assassin":
                            return Guid.Parse(TestingConstants.CardType.ASSASSIN_ID);

                        case "melee":
                            return Guid.Parse(TestingConstants.CardType.FIGHTER_ID);

                        case "ranged":
                            return Guid.Parse(TestingConstants.CardType.RANGED_ID);

                        case "tank":
                            return Guid.Parse(TestingConstants.CardType.TANK_ID);

                        case "totem":
                            return Guid.Parse(TestingConstants.CardType.TOTEM_ID);

                        default:
                            throw new NotSupportedException($"CharacterType '{characterType}' is not supported");
                    }

                default:
                    throw new NotSupportedException($"Type '{type}' is not supported");
            }
        }
    }
}