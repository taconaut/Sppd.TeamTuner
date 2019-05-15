using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    internal class CoreDataMappingProfile : BaseMappingProfile<CoreData>
    {
        public CoreDataMappingProfile()
        {
            // Entity -> DTO
            CreateEntityToDtoMap<CardTypeResponseDto>();
            CreateEntityToDtoMap<RarityResponseDto>();
            CreateEntityToDtoMap<ThemeResponseDto>();
        }
    }
}