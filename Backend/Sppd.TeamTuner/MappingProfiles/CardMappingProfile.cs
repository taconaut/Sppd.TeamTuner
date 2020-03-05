using System;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Objects;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    internal class CardMappingProfile : BaseMappingProfile<Card>
    {
        public CardMappingProfile()
        {
            // Entity -> DTO
            CreateMap<UserCard, UserCardResponseDto>()
                .ForMember(dst => dst.UserId, exp => exp.MapFrom(src => src.User.Id))
                .ForMember(dst => dst.CardId, exp => exp.MapFrom(src => src.Card.Id))
                .ForMember(dst => dst.CardName, exp => exp.MapFrom(src => src.Card.Name))
                .ForMember(dst => dst.ThemeId, exp => exp.MapFrom(src => src.Card.ThemeId))
                .ForMember(dst => dst.RarityId, exp => exp.MapFrom(src => src.Card.RarityId))
                .ForMember(dst => dst.CharacterTypeId, exp => exp.MapFrom(src => src.Card.CharacterTypeId))
                .ForMember(dst => dst.TypeId, exp => exp.MapFrom(src => src.Card.TypeId))
                .ForMember(dst => dst.Level, exp => exp.MapFrom(src => src.Level == null ? (int?) null : src.Level.Level))
                .ForMember(dst => dst.LevelLastModified, exp => exp.MapFrom(src => src.Level == null ? (DateTime?) null : src.Level.ModifiedOnUtc));
            CreateEntityToDtoMap<CardResponseDto>();

            // DTO -> Entity
        }
    }
}