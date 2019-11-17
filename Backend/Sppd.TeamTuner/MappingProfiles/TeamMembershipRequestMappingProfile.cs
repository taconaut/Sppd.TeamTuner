using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    internal class TeamMembershipRequestMappingProfile : BaseMappingProfile<TeamMembershipRequest>
    {
        public TeamMembershipRequestMappingProfile()
        {
            CreateMap<TeamMembershipRequest, TeamMembershipRequestResponseDto>()
                .ForMember(dst => dst.UserName, exp => exp.MapFrom(src => src.User.Name))
                .ForMember(dst => dst.Comment, exp => exp.MapFrom(src => src.Comment))
                .ForMember(dst => dst.TeamName, exp => exp.MapFrom(src => src.Team.Name))
                .ForMember(dst => dst.RequestDateUtc, exp => exp.MapFrom(src => src.CreatedOnUtc));
        }
    }
}