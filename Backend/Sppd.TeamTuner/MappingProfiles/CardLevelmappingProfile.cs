using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    /// <summary>
    ///     Automapper mapping profile for <see cref="CardLevel" />.
    /// </summary>
    /// <seealso cref="EntityMappingProfileBase{CardLevel}" />
    internal class CardLevelMappingProfile : EntityMappingProfileBase<CardLevel>
    {
        public CardLevelMappingProfile()
        {
            // Entity -> DTO
            CreateMap<CardLevel, CardLevelResponseDto>()
                .ForMember(dst => dst.LevelLastModified, exp => exp.MapFrom(src => src.ModifiedOnUtc));

            // DTO -> Entity
            CreateMap<CardLevelUpdateRequestDto, CardLevel>();
        }
    }
}