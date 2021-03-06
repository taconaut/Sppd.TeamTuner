﻿using System;
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
                .ForMember(dst => dst.TypeId, exp => exp.MapFrom(src => GetCardTypeId(src.Type)))
                .ForMember(dst => dst.CharacterTypeId, exp => exp.MapFrom(src => GetCharacterTypeId(src.CharacterType)))

                // Use constant value
                .ForMember(dst => dst.IsDeleted, exp => exp.MapFrom(src => false))

                // Ignore navigation properties
                .ForMember(dst => dst.Theme, exp => exp.Ignore())
                .ForMember(dst => dst.Rarity, exp => exp.Ignore())
                .ForMember(dst => dst.Type, exp => exp.Ignore())
                .ForMember(dst => dst.CharacterType, exp => exp.Ignore())

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
                    return Guid.Parse(CoreDataConstants.Theme.SCIFI_ID);

                case "adventure":
                    return Guid.Parse(CoreDataConstants.Theme.ADVENTURE_ID);

                case "fantasy":
                    return Guid.Parse(CoreDataConstants.Theme.FANTASY_ID);

                case "mystical":
                    return Guid.Parse(CoreDataConstants.Theme.MYSTICAL_ID);

                case "general":
                    return Guid.Parse(CoreDataConstants.Theme.NEUTRAL_ID);

                case "superhero":
                    return Guid.Parse(CoreDataConstants.Theme.SUPERHERO_ID);

                default:
                    throw new NotSupportedException($"Theme '{themeName}' is not supported");
            }
        }

        private static Guid GetRarityId(int rarity)
        {
            switch (rarity)
            {
                case 0:
                    return Guid.Parse(CoreDataConstants.Rarity.COMMON_ID);

                case 1:
                    return Guid.Parse(CoreDataConstants.Rarity.RARE_ID);

                case 2:
                    return Guid.Parse(CoreDataConstants.Rarity.EPIC_ID);

                case 3:
                    return Guid.Parse(CoreDataConstants.Rarity.LEGENDARY_ID);

                default:
                    throw new NotSupportedException($"Rarity '{rarity}' is not supported");
            }
        }

        private static Guid GetCardTypeId(string type)
        {
            switch (type)
            {
                case "spell":
                    return Guid.Parse(CoreDataConstants.CardType.SPELL_ID);

                case "character":
                    return Guid.Parse(CoreDataConstants.CardType.CHARACTER_ID);

                case "spawn":
                    return Guid.Parse(CoreDataConstants.CardType.SPAWN_ID);

                case "trap":
                    return Guid.Parse(CoreDataConstants.CardType.TRAP_ID);

                default:
                    throw new NotSupportedException($"Type '{type}' is not supported");
            }
        }

        private static Guid? GetCharacterTypeId(string characterType)
        {
            if (string.IsNullOrEmpty(characterType))
            {
                return null;
            }

            switch (characterType)
            {
                case "assassin":
                    return Guid.Parse(CoreDataConstants.CharacterType.ASSASSIN_ID);

                case "melee":
                    return Guid.Parse(CoreDataConstants.CharacterType.MELEE_ID);

                case "ranged":
                    return Guid.Parse(CoreDataConstants.CharacterType.RANGED_ID);

                case "tank":
                    return Guid.Parse(CoreDataConstants.CharacterType.TANK_ID);

                case "totem":
                    return Guid.Parse(CoreDataConstants.CharacterType.TOTEM_ID);

                default:
                    throw new NotSupportedException($"CharacterType '{characterType}' is not supported");
            }
        }
    }
}