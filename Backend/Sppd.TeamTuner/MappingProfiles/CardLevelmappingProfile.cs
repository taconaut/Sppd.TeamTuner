using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    internal class CardLevelMappingProfile : BaseMappingProfile<CardLevel>
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