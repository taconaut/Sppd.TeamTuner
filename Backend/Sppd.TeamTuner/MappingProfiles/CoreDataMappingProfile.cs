using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    /// <summary>
    ///     Automapper mapping profile for <see cref="CoreData" />.
    /// </summary>
    /// <seealso cref="EntityMappingProfileBase{CoreData}" />
    internal class CoreDataMappingProfile : EntityMappingProfileBase<CoreData>
    {
        public CoreDataMappingProfile()
        {
            // Entity -> DTO
            CreateEntityToDtoMap<CardTypeResponseDto>();
            CreateEntityToDtoMap<CharacterTypeResponseDto>();
            CreateEntityToDtoMap<RarityResponseDto>();
            CreateEntityToDtoMap<ThemeResponseDto>();
        }
    }
}