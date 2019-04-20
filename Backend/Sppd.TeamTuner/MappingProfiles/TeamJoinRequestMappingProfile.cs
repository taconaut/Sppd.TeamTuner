using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    internal class TeamJoinRequestMappingProfile : BaseMappingProfile<TeamJoinRequest>
    {
        public TeamJoinRequestMappingProfile()
        {
            CreateMap<TeamJoinRequest, TeamJoinRequestResponseDto>()
                .ForMember(dst => dst.Name, exp => exp.MapFrom(src => src.User.Name))
                .ForMember(dst => dst.Comment, exp => exp.MapFrom(src => src.Comment));
        }
    }
}