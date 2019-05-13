using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    internal class TeamMappingProfile : BaseMappingProfile<Team>
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