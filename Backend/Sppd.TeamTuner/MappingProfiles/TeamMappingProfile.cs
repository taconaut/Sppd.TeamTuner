using System.Linq;

using AutoMapper;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    internal class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            // Entity -> DTO
            CreateMap<Team, TeamDto>()
                .ForMember(dest => dest.Leader, expression => expression.MapFrom(src => src.Leader.Name))
                .ForMember(dest => dest.CoLeaders, expression => expression.MapFrom(src => src.CoLeaders.Select(cl => cl.Name)))
                .ForMember(dest => dest.Members, expression => expression.MapFrom(src => src.Members.Select(cl => cl.Name)));

            // DTO -> Entity
            CreateMap<TeamCreateDto, Team>();
            CreateMap<TeamDto, Team>();
        }
    }
}