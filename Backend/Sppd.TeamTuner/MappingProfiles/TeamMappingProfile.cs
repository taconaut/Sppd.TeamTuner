using System.Linq;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    internal class TeamMappingProfile : BaseMappingProfile<Team>
    {
        public TeamMappingProfile()
        {
            // Entity -> DTO
            CreateEntityToDtoMap<TeamResponseDto>()
                .ForMember(dest => dest.Leader, expression => expression.MapFrom(src => src.Leader.Name))
                .ForMember(dest => dest.CoLeaders, expression => expression.MapFrom(src => src.CoLeaders.Select(cl => cl.Name)))
                .ForMember(dest => dest.Members, expression => expression.MapFrom(src => src.Members.Select(cl => cl.Name)));

            // DTO -> Entity
            CreateMap<TeamCreateRequestDto, Team>();
            CreateDtoToEntityMap<TeamUpdateRequestDto>();
        }
    }
}