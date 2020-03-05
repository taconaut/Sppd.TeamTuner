using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    /// <summary>
    ///     Automapper mapping profile for <see cref="Team" />.
    /// </summary>
    /// <seealso cref="EntityMappingProfileBase{Team}" />
    internal class TeamMappingProfile : EntityMappingProfileBase<Team>
    {
        public TeamMappingProfile()
        {
            // Entity -> DTO
            CreateEntityToDtoMap<TeamResponseDto>();

            // DTO -> Entity
            CreateMap<TeamCreateRequestDto, Team>();
            CreateDtoToEntityMap<TeamUpdateRequestDto>();
        }
    }
}