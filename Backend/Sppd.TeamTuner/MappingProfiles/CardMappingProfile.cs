using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    internal class CardMappingProfile : BaseMappingProfile<Card>
    {
        public CardMappingProfile()
        {
            // Entity -> DTO
            CreateMap<Card, UserCardResponseDto>()
                .ForMember(dst => dst.CardId, exp => exp.MapFrom(src => src.Id))
                .ForMember(dst => dst.UserId, exp => exp.Ignore())
                .ForMember(dst => dst.Level, exp => exp.Ignore());
            CreateEntityToDtoMap<CardResponseDto>();

            // DTO -> Entity
        }
    }
}